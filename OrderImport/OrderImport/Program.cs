using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

var factory = new OrderImportContextFactory();
using var context = factory.CreateDbContext(args);



class Customer
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }

    [Column(TypeName = "decimal(8 ,2)")]
    public decimal CreditLimit { get; set; }
}

class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderTime { get; set; }

    [Column(TypeName = "decimal(8 ,2)")]
    public decimal OrderValue { get; set; }
}

class OrderImportContext : DbContext
{
    public DbSet<Customer> Customer { get; set; }

    public DbSet<Order> Order { get; set; }
    public OrderImportContext(DbContextOptions<OrderImportContext> options)
        : base(options)
    { }
}

class OrderImportContextFactory : IDesignTimeDbContextFactory<OrderImportContext>
{
    public OrderImportContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<OrderImportContext>();
        optionsBuilder
            // Uncomment the following line if you want to print generated
            // SQL statements on the console.
            //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new OrderImportContext(optionsBuilder.Options);
    }
}