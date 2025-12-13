using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;
using ILS_WPF.Models.Database;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ILS_WPF.Services
{
    public class SupplyService : ISupplyService
    {

        public async Task MakeSupplyRequestAsync(IDbContextFactory<ILSContext> dbFactory, SupplyRequest request)
        {
            using var context = await dbFactory.CreateDbContextAsync();

            // добавляем запрос в бд
            var requestEntry = await context.SupplyRequests.AddAsync(request);
            await context.SaveChangesAsync();
            // загружаем навигационное свойство
            await requestEntry.Reference(r => r.RequestUnit).LoadAsync();
            var unit = requestEntry.Entity.RequestUnit;

            if (unit == null)
                return;
            // загружаем навигационные свойства для Unit
            await LoadUnitProperties(context, unit);

            // добавляем результат запроса в бд со статусом "в обработке"
            var response = (await context.SupplyResponses.AddAsync(new SupplyResponse() { RequestId = request.Id, Status = SupplyResponseStatus.Pending })).Entity;
            await context.SaveChangesAsync();

            // Создаем копию требований

            var requirements = request.Details.Select(d=>(SupplyRequestDetail)d.Clone()).ToList();


            // Начинаем с запросившего подразделения
            var currentUnit = request.RequestUnit;

            while (currentUnit != null && requirements.Any(d => d.Count > 0))
            {
                // Загружаем текущий Unit со складом и родителем
                await LoadUnitProperties(context, currentUnit);

                if (currentUnit.AssignedWarehouse != null)
                {
                    requirements.ForEach(detail =>
                        ProcessSupplyRequestDetail(currentUnit.AssignedWarehouse, detail));
                }

                // Переходим к родителю для следующей итерации
                currentUnit = currentUnit.Parent;
            }

            var unprocessedRequirements = requirements.Where(d => d.Count > 0).ToList();


            SupplyResponseStatus responseStatus;

            if (unprocessedRequirements.Any())
            {
                bool anyChanged = unprocessedRequirements.Sum(ur=>ur.Count) < request.Details.Sum(d=>d.Count);
                responseStatus = anyChanged ? SupplyResponseStatus.Partial : SupplyResponseStatus.Denied;
            }
            else
                responseStatus = SupplyResponseStatus.Success;

            response.Status = responseStatus;
            response.UnprocessedDetails = unprocessedRequirements;

            if (responseStatus == SupplyResponseStatus.Denied)
                response.Comment = "Ни одно из вышестоящих подразделений не смогло удовлетворить запрос.";

            context.SupplyResponses.Update(response);

            await context.SaveChangesAsync();
        }

        async Task LoadUnitProperties(ILSContext context, Unit unit)
        {
            await context.Units
                .Where(u => u.Id == unit.Id)
                .Include(u => u.AssignedWarehouse)
                    .ThenInclude(w => w!.Resources)
                .Include(u => u.AssignedWarehouse)
                    .ThenInclude(w => w!.Equipments)
                .Include(u => u.Parent)
                    .ThenInclude(p => p!.AssignedWarehouse)
                .LoadAsync();
        }


        /// <summary>
        /// Списывание необходимого количества снаряжения со склада на основе переданных данных
        /// Корректировка деталей запроса в соответствии с оставшимся количеством
        /// </summary>
        /// <param name="warehouse"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        void ProcessSupplyRequestDetail(Warehouse warehouse, SupplyRequestDetail detail)
        {
            float remaining = detail.Count;

            switch (detail.MaterialType)
            {
                case MaterialType.Ammunition or MaterialType.Fuel:
                    warehouse.Resources.ForEach(r =>
                    {
                        if (remaining == 0)
                            return;

                        if (r.IsMatches(detail))
                            remaining -= r.Decrease(remaining);
                    });
                    break;
                case MaterialType.Vehicle or MaterialType.Weapon:
                    warehouse.Equipments.ForEach(e =>
                    {
                        if (remaining == 0)
                            return;

                        if (e.IsMatches(detail))
                            remaining -= e.Decrease((int)remaining);
                    });
                    break;
            }
            warehouse.Equipments.RemoveAll(e=>e.IsEmpty());
            warehouse.Resources.RemoveAll(r=>r.IsEmpty());
            detail.Count = remaining;
        }
    }
}
