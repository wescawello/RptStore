using Microsoft.EntityFrameworkCore;
using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PTCStore.Data
{
    public partial class SdContext : DbContext
    {
        public SdContext(DbContextOptions<SdContext> options)
            : base(options)
        {
        }

        public SdContext()
        {

        }

        public DbSet<Vandor> Vandors { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<VandorDevice> VandorDevices { get; set; }



        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<Formate> Formates { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<StockUnit> StockUnits { get; set; }
        public DbSet<StockOrder> StockOrders { get; set; }
        public DbSet<PickOrder> PickOrders { get; set; }
        public DbSet<PickOrderSub> PickOrderSubs { get; set; }




        public DbSet<StockOrderSub> StockOrderSubs { get; set; }
        public DbSet<CustomerInfo> CustomerInfos { get; set; }
        public DbSet<SaleOrder> SaleOrders { get; set; }
 
        public DbSet<ReturnOrder> ReturnOrders { get; set; }
        public DbSet<InfoHistory> InfoHistories { get; set; }






        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=PTCStore;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Formate>().HasOne(o => o.VandorDevice).WithMany(o => o.Formates)
             .HasForeignKey(t => new { VandorId= t.VandorId, DeviceId=t.DeviceId })
             .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VandorDevice>(e =>
            {

                e.HasKey(t => new { t.VandorId, t.DeviceId });

                e.HasOne(pt => pt.Vandor)
                .WithMany(p => p.VandorDevices)
                .HasForeignKey(pt => pt.VandorId);

                e.HasOne(pt => pt.Device)
                .WithMany(t => t.VandorDevices)
                .HasForeignKey(pt => pt.DeviceId);
            });

            modelBuilder.Entity<Vandor>(e =>
            {
                e.HasIndex(u => u.Name).IsUnique();
            });

            modelBuilder.Entity<Device>(e =>
            {
                e.HasIndex(u => u.Name).IsUnique();
            });
            modelBuilder.Entity<Formate>(e =>
            {
                e.HasMany(g => g.Purchases)
                .WithOne(s => s.Formate)
                .HasForeignKey(s => s.FormateId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Purchase>(e =>
            {
                e.HasMany(g => g.Barcodes)
              .WithOne(s => s.Purchase)
              .HasForeignKey(s => s.PurchaseId)
              .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(u => u.ApplyNumber).IsUnique();
            });

            modelBuilder.Entity<Barcode>(e =>
            {
                e.HasIndex(u => u.BarcodeValue)
                               .IsUnique();
            
                e.HasOne(u=>u.StockUnit).WithMany().HasForeignKey(u=>u.StockUnitId).OnDelete(DeleteBehavior.NoAction);
            });
               
            //.HasAlternateKey(o => o.BarcodeValue);


            modelBuilder.Entity<StockUnit>(e =>
            {
                e.HasIndex(u => u.Name).IsUnique();
                e.Property(o => o.OOXX)
                .HasConversion(
                   v => JsonSerializer.Serialize(v, default),
            v => JsonSerializer.Deserialize<string[]>(v, default));


            });



            modelBuilder.Entity<StockOrder>(e =>
            {
                e.HasOne(o => o.OrgStockUnit).WithMany().HasForeignKey(o => o.OrgStockUnitId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(o => o.StockUnit).WithMany().HasForeignKey(o => o.StockUnitId).OnDelete(DeleteBehavior.NoAction);
                e.Property(o => o.Summary).HasConversion(
                    v => JsonSerializer.Serialize(v, default),
                    v => JsonSerializer.Deserialize<string[]>(v, default));
                e.HasIndex(u => u.ApplyNumber).IsUnique();
            });
            modelBuilder.Entity<StockOrderSub>(e => {

                e.HasOne(o => o.StockOrder).WithMany(o => o.StockOrderSubs).HasForeignKey(o => o.StockOrderId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(o => o.Barcode).WithMany().HasForeignKey(o => o.BarcodeId).OnDelete(DeleteBehavior.NoAction);

            });


            modelBuilder.Entity<CustomerInfo>(e =>
            {
                e.HasIndex(u => u.Name).IsUnique();
            });

            modelBuilder.Entity<SaleOrder>(e =>
            {
                e.HasIndex(u => u.ApplyNumber).IsUnique();
                e.HasMany(u => u.PickOrders).WithOne(p => p.SaleOrder).HasForeignKey(g => g.SaleOrderId).OnDelete(DeleteBehavior.NoAction);
                e.OwnsOne(p => p.Customer);
            });

            //modelBuilder.Entity<SaleOrderSub>(e => {

            //    e.HasOne(o => o.SaleOrder).WithMany(o => o.SaleOrderSubs).HasForeignKey(o => o.SaleOrderId).OnDelete(DeleteBehavior.Cascade);
            //    e.HasOne(o => o.Barcode).WithMany().HasForeignKey(o => o.BarcodeId).OnDelete(DeleteBehavior.NoAction);
            //    e.HasOne(o => o.ReturnOrderSub).WithOne(x=>x.SaleOrderSub).HasForeignKey<ReturnOrderSub>(o => o.ReturnOrderId).OnDelete(DeleteBehavior.NoAction);

            //});
            modelBuilder.Entity<PickOrder>(e =>
            {
               

                e.HasOne(o => o.SaleOrder).WithMany(o => o.PickOrders).HasForeignKey(o => o.SaleOrderId).OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<PickOrderSub>(e =>
            {
                e.HasOne(o => o.PickOrder).WithMany(o => o.PickOrderSubs).HasForeignKey(o => o.PickOrderId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(o => o.Barcode).WithMany().HasForeignKey(o => o.BarcodeId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(o => o.ReturnOrder).WithMany(x => x.ReturnOrderSubs).HasForeignKey(o => o.ReturnOrderId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ReturnOrder>(e =>
            {
                e.HasIndex(u => u.ApplyNumber).IsUnique();
            });
           

        }
    }
}
 
