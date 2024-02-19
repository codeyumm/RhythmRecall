using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace RhythmRecall.Models
{
    public class Track
    {
        /* 

Table Track
{
    TrackID int [pk]
    Title varchar
  Artist varchar
  Album varchar
  ReleaseDate date
}
        we need bridign tablef g
 */

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string AlbumArt { get; set; }

        public DateTime? ReleaseDate { get; set; } = new DateTime(2024, 12, 12);

        // TrackList can have many tracks
        public ICollection<TrackList> TrackLists { get; set;}

        // Reviews can have many tracks
        public ICollection<Review> Reviews { get; set; }
    }

    // DTO stands for data tranfer object
    // we can omit some data which we dont want to send with API
    public class TrackDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ListenLater { get; set; }
    }
}


