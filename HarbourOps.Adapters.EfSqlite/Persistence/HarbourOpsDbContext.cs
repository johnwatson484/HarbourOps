using HarbourOps.Domain;
using HarbourOps.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace HarbourOps.Adapters.EfSqlite.Persistence;

public sealed class HarbourOpsDbContext(DbContextOptions<HarbourOpsDbContext> options)
    : DbContext(options)
{
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<ServiceItem> ServiceItems => Set<ServiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceItem>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).HasMaxLength(32).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();

            builder.OwnsOne(x => x.UnitPrice, money =>
            {
                money.Property(x => x.Amount).HasColumnName("UnitPriceAmount");
                money.Property(x => x.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3);
            });

            builder.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Booking>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerEmail)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(x => x.VesselName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.ContainerNumber)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32);

            builder.Property(x => x.PaymentReference)
                .HasMaxLength(200);

            builder.Property(x => x.CreatedAtUtc)
                .HasConversion(
                    value => value.ToUnixTimeMilliseconds(),
                    value => DateTimeOffset.FromUnixTimeMilliseconds(value));

            builder.OwnsMany(x => x.Lines, line =>
            {
                line.ToTable("BookingLines");
                line.WithOwner().HasForeignKey("BookingId");
                line.HasKey(x => x.Id);

                line.Property(x => x.ServiceName)
                    .HasMaxLength(200)
                    .IsRequired();

                line.OwnsOne(x => x.UnitPrice, money =>
                {
                    money.Property(x => x.Amount).HasColumnName("UnitPriceAmount");
                    money.Property(x => x.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3);
                });
            });

            builder.Metadata.FindNavigation(nameof(Booking.Lines))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        SeedServices(modelBuilder);
    }

    private static void SeedServices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceItem>().HasData(
            new
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Code = "REEFER-MON",
                Name = "Reefer Container Monitoring",
                Description = "Temperature monitoring and daily check for refrigerated containers.",
                IsActive = true
            },
            new
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Code = "VGM-WEIGH",
                Name = "Verified Gross Mass Weighing",
                Description = "Certified container weighing for export compliance.",
                IsActive = true
            },
            new
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Code = "CUSTOMS-HOLD",
                Name = "Customs Hold Handling",
                Description = "Special handling for containers under customs inspection.",
                IsActive = true
            },
            new
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Code = "PRIORITY-GATE",
                Name = "Priority Gate Appointment",
                Description = "Priority truck gate processing during peak terminal hours.",
                IsActive = true
            });

        modelBuilder.Entity<ServiceItem>().OwnsOne(x => x.UnitPrice).HasData(
            new
            {
                ServiceItemId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Amount = 125m,
                Currency = "GBP"
            },
            new
            {
                ServiceItemId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Amount = 85m,
                Currency = "GBP"
            },
            new
            {
                ServiceItemId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Amount = 210m,
                Currency = "GBP"
            },
            new
            {
                ServiceItemId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Amount = 60m,
                Currency = "GBP"
            });
    }
}
