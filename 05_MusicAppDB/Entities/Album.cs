using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_MusicAppDB.Entities
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Url]
        [MaxLength(300)]
        public string CoverImageUrl { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }



}
