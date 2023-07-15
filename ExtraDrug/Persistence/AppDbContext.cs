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

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.UserDrugs)
            .WithOne(ud => ud.User)
            .HasForeignKey(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Drug>()
            .HasMany(d => d.DrugUsers)
            .WithOne(ud => ud.Drug)
            .HasForeignKey(ud => ud.DrugId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserDrug>()
            .HasMany(ud => ud.Photos)
            .WithOne(udp => udp.UserDrug)
            .HasForeignKey(udp => udp.UserDrugId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RequestItem>().
            HasKey(ri => new { ri.UserDrugId, ri.DrugRequestId });

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.RequestsAsReciever)
            .WithOne(rq => rq.Receiver)
            .HasForeignKey(rq => rq.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
           .HasMany(u => u.RequestsAsDoner)
           .WithOne(rq => rq.Donor)
           .HasForeignKey(rq => rq.DonorId)
           .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<DrugRequest>()
            .HasMany(r => r.RequestItems)
            .WithOne(ri => ri.DrugRequest)
            .HasForeignKey(ri => ri.DrugRequestId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<UserDrug>()
            .HasMany(ud => ud.RequestItems)
            .WithOne(ri => ri.UserDrug)
            .HasForeignKey(ri => ri.UserDrugId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DrugRequest>()
            .Property(dr => dr.State)
            .HasConversion(s => s.ToString(), s => Enum.Parse<RequestState>(s));
    }
    public virtual DbSet<Drug> Drugs { get; set; }
    public virtual DbSet<DrugCategory> DrugCategories { get; set; }
    public virtual DbSet<DrugCompany> DrugCompanies { get; set; }
    public virtual DbSet<DrugType> DrugTypes { get; set; }
    public virtual DbSet<EffectiveMatrial> EffectiveMatrials { get; set; }
    public virtual DbSet<UserDrug> UsersDrugs { get; set; }
    public virtual DbSet<UserDrugPhoto> UserDrugsPhotos { get; set; }
    public virtual DbSet<DrugRequest> DrugRequests { get; set; }

}
