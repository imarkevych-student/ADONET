using _06_ShopsDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;


namespace _06_ShopsDb
{
    public class ShopDbContext : DbContext
    {
        private readonly string _connectionString;

        public ShopDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }


        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Cities)
                .WithOne(c => c.Country)
                .HasForeignKey(c => c.CountryId);

            modelBuilder.Entity<City>()
                .HasMany(c => c.Shops)
                .WithOne(s => s.City)
                .HasForeignKey(s => s.CityId);

            modelBuilder.Entity<Shop>()
                .HasMany(s => s.Workers)
                .WithOne(w => w.Shop)
                .HasForeignKey(w => w.ShopId);

            modelBuilder.Entity<Position>()
                .HasMany(p => p.Workers)
                .WithOne(w => w.Position)
                .HasForeignKey(w => w.PositionId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Україна" },
                new Country { Id = 2, Name = "Польща" },
                new Country { Id = 3, Name = "Німеччина" },
                new Country { Id = 4, Name = "Франція" },
                new Country { Id = 5, Name = "Італія" }
            );

            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Київ", CountryId = 1 },
                new City { Id = 2, Name = "Львів", CountryId = 1 },
                new City { Id = 3, Name = "Варшава", CountryId = 2 },
                new City { Id = 4, Name = "Берлін", CountryId = 3 },
                new City { Id = 5, Name = "Париж", CountryId = 4 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Електроніка" },
                new Category { Id = 2, Name = "Одяг" },
                new Category { Id = 3, Name = "Продукти" },
                new Category { Id = 4, Name = "Іграшки" },
                new Category { Id = 5, Name = "Канцелярія" }
            );

            modelBuilder.Entity<Position>().HasData(
                new Position { Id = 1, Name = "Менеджер" },
                new Position { Id = 2, Name = "Касир" },
                new Position { Id = 3, Name = "Охоронець" },
                new Position { Id = 4, Name = "Прибиральник" },
                new Position { Id = 5, Name = "Технік" }
            );

            modelBuilder.Entity<Shop>().HasData(
                new Shop { Id = 1, Name = "Магазин A", Address = "вул. Хрещатик 1", CityId = 1, ParkingArea = 50 },
                new Shop { Id = 2, Name = "Магазин B", Address = "вул. Головна 12", CityId = 2, ParkingArea = 30 },
                new Shop { Id = 3, Name = "Магазин C", Address = "вул. Центральна 5", CityId = 3, ParkingArea = 40 },
                new Shop { Id = 4, Name = "Магазин D", Address = "вул. Шевченка 22", CityId = 4, ParkingArea = 60 },
                new Shop { Id = 5, Name = "Магазин E", Address = "вул. Рішельєвська 8", CityId = 5, ParkingArea = 35 }
            );
        }


    }

    public class ShopDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
    {
        public ShopDbContext CreateDbContext(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ShopDb"].ConnectionString;
            return new ShopDbContext(connectionString);
        }
    }


}
