﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication7.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SapphireDataBaseEntities : DbContext
    {
        public SapphireDataBaseEntities()
            : base("name=SapphireDataBaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public virtual DbSet<ExpenseDetail> ExpenseDetails { get; set; }
        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryRecord> InventoryRecords { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductionCategory> ProductionCategories { get; set; }
        public virtual DbSet<Revenue> Revenues { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockCreationHistory> StockCreationHistories { get; set; }
        public virtual DbSet<Thekedar> Thekedars { get; set; }
        public virtual DbSet<TotalInventory> TotalInventories { get; set; }
        public virtual DbSet<TotalRevenue> TotalRevenues { get; set; }
        public virtual DbSet<WeekNumber> WeekNumbers { get; set; }
    }
}