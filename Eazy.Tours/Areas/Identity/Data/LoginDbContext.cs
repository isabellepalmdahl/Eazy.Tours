using Eazy.Tours.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eazy.Tours.Areas.Identity.Data;

public class LoginDbContext : IdentityDbContext<ApplicationUser>
{
    public LoginDbContext(DbContextOptions<LoginDbContext> options)
        : base(options)
    {
    }
    //public DbSet<User> Users { get; set; }
    //public DbSet<Category> Categories { get; set; }
    //public DbSet<Product> Products { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    //public DbSet<Cart> Carts { get; set; }
    //public DbSet<OrderHeader> OrderHeaders { get; set; }
    //public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfirmation());
    }
}

public class ApplicationUserEntityConfirmation : IEntityTypeConfiguration<ApplicationUser>
{
    void IEntityTypeConfiguration<ApplicationUser>.Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(128);
        builder.Property(u => u.LastName).HasMaxLength(128);
    }
}