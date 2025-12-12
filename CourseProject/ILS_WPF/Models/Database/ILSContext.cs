using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Equipments;
using ILS_WPF.Models.Core.Requests;
using ILS_WPF.Models.Core.Resources;
using Microsoft.EntityFrameworkCore;

namespace ILS_WPF.Models.Database
{
    public class ILSContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Personnel { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<SupplyRequest> SupplyRequests { get; set; }
        public DbSet<SupplyRequestDetail> SupplyRequestDetails { get; set; }
        public DbSet<SupplyResponse> SupplyResponses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Equipment> Equipment { get; set; }

        public ILSContext()
            => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ILSConnectionString"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>()
                 .ToTable("Resources")
                 .HasDiscriminator(r=>r.MaterialType)
                 .HasValue<Ammunition>(MaterialType.Ammunition)
                 .HasValue<Fuel>(MaterialType.Fuel);

            modelBuilder.Entity<Equipment>()
                .ToTable("Equipment")
                .HasDiscriminator(e=>e.MaterialType)
                .HasValue<Weapon>(MaterialType.Weapon)
                .HasValue<Vehicle>(MaterialType.Vehicle);

            // Staff => Unit (солдаты в подразделении)
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Unit)
                .WithMany(u => u.Personnel)
                .HasForeignKey(s => s.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Unit => Commander (командир подразделения)
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Commander)
                .WithMany()
                .HasForeignKey(u => u.CommanderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Unit иерархия (родитель-потомки)
            modelBuilder.Entity<Unit>()
                .HasMany(u => u.Children)
                .WithOne(u => u.Parent)
                .HasForeignKey(u => u.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Unit => AssignedWarehouse (прикрепленный склад)
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.AssignedWarehouse)
                .WithMany()
                .HasForeignKey(u => u.AssignedWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Unit => SupplyRequests (заявки подразделения)
            modelBuilder.Entity<Unit>()
                .HasMany(u => u.SupplyRequests)
                .WithOne(sr => sr.RequestUnit)
                .HasForeignKey(sr => sr.RequestUnitId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // SupplyRequest => Details (детали заявки)
            modelBuilder.Entity<SupplyRequest>()
                .HasMany(sr => sr.Details)
                .WithOne(d => d.SupplyRequest)
                .HasForeignKey(d => d.SupplyRequestId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // SupplyRespone => Details (неудовлетворенные детали заявки)
            modelBuilder.Entity<SupplyResponse>()
                .HasMany(sr => sr.UnprocessedDetails)
                .WithOne(d => d.SupplyResponse)
                .HasForeignKey(d => d.SupplyResponseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // SupplyResponse => SupplyRequest (ответ на заявку)
            modelBuilder.Entity<SupplyResponse>()
                .HasOne(sr => sr.Request)
                .WithMany()
                .HasForeignKey(sr => sr.RequestId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Warehouse => Resources (ресурсы на складе)
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Warehouse)
                .WithMany(w => w.Resources)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Warehouse => Equipment (оборудование на складе)
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Warehouse)
                .WithMany(w => w.Equipments)
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
