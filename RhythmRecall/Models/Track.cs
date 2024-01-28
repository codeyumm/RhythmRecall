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
        we need bridign tablef gvf
 */

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public DateTime ReleaseDate { get; set; }

    }
}


