using DDAC.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DDAC.Models;

namespace DDAC.Data;

public class DDACContext : IdentityDbContext<DDACUser>
{
    public DDACContext(DbContextOptions<DDACContext> options)
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

    public DbSet<DDAC.Models.Activities> Activities { get; set; } = default!;
    public DbSet<DDAC.Models.Enroll> Enroll { get; set; } = default!;

    public DbSet<DDAC.Models.Attendance> Attendance { get; set; } = default!;
}
