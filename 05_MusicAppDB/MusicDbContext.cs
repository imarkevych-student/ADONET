using Microsoft.EntityFrameworkCore;
using _05_MusicAppDB.Entities;


namespace _05_MusicAppDB
{
    internal class MusicDbContext : DbContext
    {
        public MusicDbContext()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;
                                          Initial Catalog=MusicApp_Db;
                                          Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().HasData(new Country[]
            {
                new Country { Id = 1, Name = "USA" },
                new Country { Id = 2, Name = "Ukraine" },
                new Country { Id = 3, Name = "UK" },
                new Country { Id = 4, Name = "Germany" }
            });

            modelBuilder.Entity<Genre>().HasData(new Genre[]
            {
                new Genre { Id = 1, Name = "Pop" },
                new Genre { Id = 2, Name = "Rock" },
                new Genre { Id = 3, Name = "Jazz" },
                new Genre { Id = 4, Name = "Electronic" }
            });

            modelBuilder.Entity<Artist>().HasData(new Artist[]
            {
                new Artist { Id = 1, FirstName = "John", LastName = "Doe", CountryId = 1 },
                new Artist { Id = 2, FirstName = "Anna", LastName = "Koval", CountryId = 2 },
                new Artist { Id = 3, FirstName = "Liam", LastName = "Smith", CountryId = 3 },
                new Artist { Id = 4, FirstName = "Marta", LastName = "Schmidt", CountryId = 4 }
            });

            modelBuilder.Entity<Album>().HasData(new Album[]
            {
                new Album { Id = 1, Name = "Dreams", Year = 2020, ArtistId = 1, GenreId = 1, CoverImageUrl = "https://unsplash.com/photos/a-red-and-white-abstract-design-with-a-black-background-wMxCKg1_i1M" },
                new Album { Id = 2, Name = "Skyline", Year = 2021, ArtistId = 2, GenreId = 2, CoverImageUrl = "https://unsplash.com/photos/red-illuminated-sign-with-symbols-on-sandy-ground-wOFisBZhjYI" },
                new Album { Id = 3, Name = "Echoes", Year = 2022, ArtistId = 3, GenreId = 3, CoverImageUrl = "https://unsplash.com/photos/diner-with-american-flag-decor-and-illuminated-menu-signs-LFUUPn8fLb4" },
                new Album { Id = 4, Name = "Pulse", Year = 2023, ArtistId = 4, GenreId = 4, CoverImageUrl = "https://unsplash.com/photos/palm-tree-illuminated-with-red-light-at-night-O-zy0EalUaY" }
            });

            modelBuilder.Entity<Track>().HasData(new Track[]
            {
                new Track { Id = 1, Name = "Sunrise", Duration = TimeSpan.FromMinutes(3), AlbumId = 1 },
                new Track { Id = 2, Name = "Moonlight", Duration = TimeSpan.FromMinutes(4), AlbumId = 1 },
                new Track { Id = 3, Name = "Freedom", Duration = TimeSpan.FromMinutes(3), AlbumId = 2 },
                new Track { Id = 4, Name = "Storm", Duration = TimeSpan.FromMinutes(5), AlbumId = 2 },
                new Track { Id = 5, Name = "Whispers", Duration = TimeSpan.FromMinutes(4), AlbumId = 3 },
                new Track { Id = 6, Name = "Reflections", Duration = TimeSpan.FromMinutes(6), AlbumId = 3 },
                new Track { Id = 7, Name = "Voltage", Duration = TimeSpan.FromMinutes(3), AlbumId = 4 },
                new Track { Id = 8, Name = "Neon", Duration = TimeSpan.FromMinutes(4), AlbumId = 4 }
            });

            modelBuilder.Entity<Playlist>().HasData(new Playlist[]
            {
                new Playlist { Id = 1, Name = "Chill Vibes", Category = "Relax", CoverImageUrl = "https://unsplash.com/photos/a-disco-ball-hangs-from-a-white-ceiling-acITggTrVl4" },
                new Playlist { Id = 2, Name = "Workout Boost", Category = "Fitness", CoverImageUrl = "https://unsplash.com/photos/empty-room-with-wooden-floor-and-large-window-4YhNRgL59Fc" },
                new Playlist { Id = 3, Name = "Jazz Nights", Category = "Evening", CoverImageUrl = "https://unsplash.com/photos/hoh-visitor-center-sign-in-a-forest-4TU3ch5eL3c" }
            });

            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Tracks)
                .WithMany(t => t.Playlists)
                .UsingEntity(j => j.HasData(
                    new { PlaylistsId = 1, TracksId = 1 },
                    new { PlaylistsId = 1, TracksId = 2 },
                    new { PlaylistsId = 2, TracksId = 4 },
                    new { PlaylistsId = 2, TracksId = 7 },
                    new { PlaylistsId = 3, TracksId = 5 },
                    new { PlaylistsId = 3, TracksId = 6 }
                ));
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
    }

}
