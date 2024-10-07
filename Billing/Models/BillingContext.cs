using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Billing.Models;

public class BillingContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Type> Types { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Stocks> Stocks { get; set; }

    public BillingContext(DbContextOptions<BillingContext> options): base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().HasData(
               new Client { Id = 1, Name = "John Doe", Phone = "123456789", Number = 1001, Address = "123 Street" },
               new Client { Id = 2, Name = "Jane Smith", Phone = "987654321", Number = 1002, Address = "456 Avenue" }
           );
        modelBuilder.Entity<Client>().HasIndex(c => c.Name).IsUnique();
         modelBuilder.Entity<Client>().Property(c => c.Name).HasColumnType("varchar(256)").UseCollation("SQL_Latin1_General_CP1_CI_AS");  



        modelBuilder.Entity<Stocks>().HasData(
               new Stocks { Id = 1,  Quntity = 50,ItemsId=1 },
               new Stocks { Id = 2,  Quntity = 100, ItemsId = 2 }
               
           );


        
        modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Name = "ABC Corp", Notes = "First company" },
            new Company { Id = 2, Name = "XYZ Inc", Notes = "Second company" }
        );
        modelBuilder.Entity<Company>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Company>().Property(c => c.Name).HasColumnType("varchar(256)").UseCollation("SQL_Latin1_General_CP1_CI_AS");




        // Seed data for Units
        modelBuilder.Entity<Unit>().HasData(
            new Unit { Id = 1, Name = "Kilogram", Notes = "Weight" },
            new Unit { Id = 2, Name = "Liter", Notes = "Volume" }
        );
        modelBuilder.Entity<Unit>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Unit>().Property(c => c.Name).HasColumnType("varchar(256)").UseCollation("SQL_Latin1_General_CP1_CI_AS");

        // Seed data for Types
        modelBuilder.Entity<Type>().HasData(
            new Type { Id = 1, Name = "Electronics", Notes = "Electrical devices", CompanyId = 1 },
            new Type { Id = 2, Name = "Furniture", Notes = "Home furniture", CompanyId = 2 }
        );
        modelBuilder.Entity<Type>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Type>().Property(c => c.Name).HasColumnType("varchar(256)").UseCollation("SQL_Latin1_General_CP1_CI_AS");

        // Seed data for Items
        modelBuilder.Entity<Item>().HasData(
            new Item
            {
                Id = 1,
                Name = "Laptop",
                Notes = "15-inch display",
                BuyingPrice = 1000,
                SellingPrice = 1200,
                CompanyId = 1,
                TypeId = 1,
                UnitId=1
                
            },
            new Item
            {
                Id = 2,
                Name = "Chair",
                Notes = "Office chair",
                BuyingPrice = 50,
                SellingPrice = 80,
                CompanyId = 2,
                TypeId = 2,
                UnitId=2
                
            }
        );
        modelBuilder.Entity<Item>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Item>().Property(c => c.Name).HasColumnType("varchar(256)").UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Invoice>().HasData(
            new Invoice
            {
                Id = 1,
                Number = "INV001",
                BillTotal = 1100,
                BillDate = DateTime.Now,
                DiscountPercentage = 10,
                Net = 990,
                PaidUp = 500,
                Rest = 490,
                ClientId = 1,
                
            },
            new Invoice
            {
                Id = 2,
                Number = "INV002",
                BillTotal = 1500,
                BillDate = DateTime.Now,
                DiscountPercentage = 5,
                Net = 1425,
                PaidUp = 1000,
                Rest = 425,
                ClientId = 2,
               
            }
        );

        // Seed data for InvoiceItems
        modelBuilder.Entity<InvoiceItem>().HasData(
            new InvoiceItem
            {
                Id = 1,
                InvoiceId = 1,
                ItemId = 1,
                
                Quantity = 1,
                Price = 1100
            },
            new InvoiceItem
            {
                Id = 2,
                InvoiceId = 2,
                ItemId = 2,
               
                Quantity = 2,
                Price = 150
            }
        );
        base.OnModelCreating(modelBuilder);
    }
}


