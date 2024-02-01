using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RhythmRecall.Models
{
    public class TrackList
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Userss")]
        public int UserId { get; set; }
        public virtual User Userss { get; set; }

        [ForeignKey("Tracks")]

        public int TrackId { get; set; }
        public virtual Track Tracks { get; set; }


        public int ListenLater { get; set; }

        public int Discovered { get; set; }

    }

    public class TrackListDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Username { get; set; }
    }
}