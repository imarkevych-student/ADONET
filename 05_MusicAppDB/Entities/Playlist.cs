using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_MusicAppDB.Entities
{
    public class Playlist
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Url]
        [MaxLength(300)]
        public string CoverImageUrl { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }




}
