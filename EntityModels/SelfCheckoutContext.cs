using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SelfCheckoutAPI.EntityModels;

public partial class SelfCheckoutContext : DbContext
{
    public SelfCheckoutContext()
    {
    }

    public SelfCheckoutContext(DbContextOptions<SelfCheckoutContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionItem> TransactionItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=54320;Database=Self_checkout;Username=postgres;Password=dbpassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Pin).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.Pin)
                .ValueGeneratedNever()
                .HasColumnName("pin");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ItemId }).HasName("cart_item_pkey");

            entity.ToTable("cart_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemId)
                .HasMaxLength(200)
                .HasColumnName("item_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cart_item_item");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pkey");

            entity.ToTable("department");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(240)
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Upc).HasName("item_pkey");

            entity.ToTable("item");

            entity.Property(e => e.Upc)
                .HasMaxLength(50)
                .HasColumnName("upc");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.IsDiscontinued).HasColumnName("is_discontinued");
            entity.Property(e => e.IsTaxed).HasColumnName("is_taxed");
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .HasColumnName("item_name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Department).WithMany(p => p.Items)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_department");
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.TaxRate).HasName("tax_pkey");

            entity.ToTable("tax");

            entity.Property(e => e.TaxRate).HasColumnName("tax_rate");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_pkey");

            entity.ToTable("transaction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DatePurchased)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_purchased");
        });

        modelBuilder.Entity<TransactionItem>(entity =>
        {
            entity.HasKey(e => new { e.ItemId, e.TransactionId }).HasName("transaction_item_pkey");

            entity.ToTable("transaction_item");

            entity.Property(e => e.ItemId)
                .HasColumnType("character varying")
                .HasColumnName("item_id");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.PriceBought).HasColumnName("price_bought");
            entity.Property(e => e.QuantityBought).HasColumnName("quantity_bought");

            entity.HasOne(d => d.Item).WithMany(p => p.TransactionItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_transaction_item_item");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionItems)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_transaction_item_transaction");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
