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
                 .HasDiscriminator<MaterialType>("ResourceType")
                 .HasValue<Ammunition>(MaterialType.Ammunition)
                 .HasValue<Fuel>(MaterialType.Fuel);

            modelBuilder.Entity<Equipment>()
                .ToTable("Equipment")
                .HasDiscriminator<MaterialType>("EquipmentType")
                .HasValue<Weapon>(MaterialType.Weapon)
                .HasValue<Vehicle>(MaterialType.Vehicle);

            // Staff => Unit (солдаты в подразделении)
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Unit)
                .WithMany(u => u.Personnel)
                .HasForeignKey(s => s.UnitId)
                .OnDelete(DeleteBehavior.SetNull);

            // Unit => Commander (командир подразделения)
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Commander)
                .WithMany()
                .HasForeignKey(u => u.CommanderId)
                .OnDelete(DeleteBehavior.SetNull);

            // Unit иерархия (родитель-потомки)
            modelBuilder.Entity<Unit>()
                .HasMany(u => u.Children)
                .WithOne(u => u.Parent)
                .HasForeignKey(u => u.ParentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Unit => AssignedWarehouse (прикрепленный склад)
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.AssignedWarehouse)
                .WithMany()
                .HasForeignKey(u => u.AssignedWarehouseId)
                .OnDelete(DeleteBehavior.SetNull);

            // Unit => SupplyRequests (заявки подразделения)
            modelBuilder.Entity<Unit>()
                .HasMany(u => u.SupplyRequests)
                .WithOne(sr => sr.RequestUnit)
                .HasForeignKey(sr => sr.RequestUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            // SupplyRequest => Details (детали заявки)
            modelBuilder.Entity<SupplyRequest>()
                .HasMany(sr => sr.Details)
                .WithOne(d => d.SupplyRequest)
                .HasForeignKey(d => d.SupplyRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // SupplyResponse => SupplyRequest (ответ на заявку)
            modelBuilder.Entity<SupplyResponse>()
                .HasOne(sr => sr.Request)
                .WithMany()
                .HasForeignKey(sr => sr.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Warehouse => Resources (ресурсы на складе)
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Warehouse)
                .WithMany(w => w.Resources)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Warehouse => Equipment (оборудование на складе)
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Warehouse)
                .WithMany(w => w.Equipments)
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
