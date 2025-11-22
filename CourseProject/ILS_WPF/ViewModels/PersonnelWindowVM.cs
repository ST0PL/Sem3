using ILS_WPF.Models.Core;
using ILS_WPF.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ILS_WPF.ViewModels
{
    public class PersonnelWindowVM : BaseVM
    {
        private IDbContextFactory<ILSContext> _dbFactory;
        public PersonnelWindowVM(IDbContextFactory<ILSContext> dbFactory, Staff? soldier = null)
        {
            _dbFactory = dbFactory;
        }

        async Task LoadData()
        {

        }
    }
}
