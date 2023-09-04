using InvMng_InfTech.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InvMng_InfTech.Models.Order;
using InvMng_InfTech.Models.Masters;
using InvMng_InfTech.Models.Location;

namespace InvMng_InfTech.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    

    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<InvMng_InfTech.Models.Items> Items { get; set; } = default!;

    public DbSet<InvMng_InfTech.Models.Order.Order> Order { get; set; } = default!;
    public DbSet<InvMng_InfTech.Models.Order.Customers.Customer> Customers { get; set; } = default!;

    public DbSet<InvMng_InfTech.Models.Order.OrderItem> OrderItems { get; set; } = default!;

    public DbSet<InvMng_InfTech.Models.Masters.SupplyMaster> SupplyMaster { get; set; } = default!;

    public DbSet<InvMng_InfTech.Models.Location.LocationMaster> LocationMaster { get; set; } = default!;

    public DbSet<InvMng_InfTech.Models.Masters.InventoryMaster> InventoryMaster { get; set; } = default!;
    
    public DbSet<InvMng_InfTech.Models.Masters.SubLocationMaster> SubLocationMaster { get; set; } = default!;

    public DbSet<InvMng_InfTech.Models.Masters.LogMaster> LogMaster { get; set; } = default!;

    public DbSet<InvMng_InfTech.Models.Masters.PartsMaster> PartsMaster { get; set; } = default!;
}
