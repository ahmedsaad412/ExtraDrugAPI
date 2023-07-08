using ExtraDrug.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence;

public class AppDbContext:IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Drug>()
            .HasOne(d => d.Company)
            .WithMany(c => c.Drugs)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Drug>()
            .HasOne(d => d.DrugCategory)
            .WithMany(c => c.Drugs)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Drug>()
            .HasOne(d => d.DrugType)
            .WithMany(t => t.Drugs)
            .HasForeignKey(d => d.TypeId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Drug>()
            .HasMany(d => d.EffectiveMatrials)
            .WithMany(em => em.InDrugs);

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();
            
    }
    public virtual DbSet<Drug> Drugs { get; set; }
    public virtual DbSet<DrugCategory> DrugCategories { get; set; }
    public virtual DbSet<DrugCompany> DrugCompanies { get; set; }
    public virtual DbSet<DrugType> DrugTypes { get; set; }
    public virtual DbSet<EffectiveMatrial> EffectiveMatrials { get; set; }
}
