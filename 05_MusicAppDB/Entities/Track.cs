using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_MusicAppDB.Entities
{
    public class Track
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [ForeignKey("Album")]
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public ICollection<Playlist> Playlists { get; set; }
    }



}
