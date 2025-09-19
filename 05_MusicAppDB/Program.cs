using _05_MusicAppDB.Entities;

namespace _05_MusicAppDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MusicDbContext context = new MusicDbContext();

            var playlist = context.Playlists.Find(1);
            Console.WriteLine($"Playlist: {playlist.Name} ({playlist.Category})");

            foreach (var track in context.Tracks)
            {
                Console.WriteLine($"Track: {track.Name} - {track.Duration}");
            }

            var newPlaylist = new Playlist
            {
                Name = "Evening Chill",
                Category = "Ambient",
                CoverImageUrl = "https://example.com/evening.jpg",
                Tracks = new List<Track> { context.Tracks.Find(1) }
            };

            context.Playlists.Add(newPlaylist);
            context.SaveChanges();

            Console.WriteLine("New playlist added.");
        }
    }


}
