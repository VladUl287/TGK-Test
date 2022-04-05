using Microsoft.EntityFrameworkCore;
using TestApi.Database.Models;

namespace TestApi.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PersonalAccount> PersonalAccounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasData(new User
                {
                    Id = 1,
                    Email = "e@mail.ru",
                    Password = "rqhdJQb/Oi7AvOFUJsnFlo99n6F7ct0B+Sgudw7kNMM="
                });
            });

            modelBuilder.Entity<PersonalAccount>(entity =>
            {
                entity.HasKey(e => e.Number);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.PersonalAccounts)
                    .HasForeignKey(e => e.UserId);

                entity.HasData(new PersonalAccount[]
                {
                    new PersonalAccount
                    {
                        Number = Guid.NewGuid(),
                        UserId = 1,
                        CurrencyId = 1,
                        Value = 2000
                    },
                    new PersonalAccount
                    {
                        Number = Guid.NewGuid(),
                        UserId = 1,
                        CurrencyId = 2,
                        Value = 5000
                    }
                });
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DigitalСode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LetterCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Sign)
                   .IsRequired();

                entity.HasData(new Currency[]
                {
                    new Currency
                    {
                        Id = 1,
                        DigitalСode = "643",
                        LetterCode = "RUB",
                        Name = "Рубли",
                        Sign = '₽'
                    },
                    new Currency
                    {
                        Id = 2,
                        DigitalСode = "208",
                        LetterCode = "HKD",
                        Name = "Условные единицы",
                        Sign = '₵'
                    }
                });
            });

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasData(new ExchangeRate[]
                {
                    new ExchangeRate
                    {
                        Id = 1,
                        CurrencyId = 1,
                        SecondCurrencyId = 2,
                        Rate = 0.012M
                    },
                    new ExchangeRate
                    {
                        Id = 2,
                        CurrencyId = 2,
                        SecondCurrencyId = 1,
                        Rate = 83.3333333M
                    }
                });
            });
        }
    }
}