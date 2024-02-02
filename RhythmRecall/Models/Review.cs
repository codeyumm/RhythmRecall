using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhythmRecall.Models
{
    public class Review
    {
        /* 
             Cols of Review table
                 ReviewId - pk
                 TrackId - fk
                 UserId - fk
                 Title
                 Content
                 Rating 
        */

        [Key]
        public int Id { get; set; }

        [ForeignKey("Tracks")]
        public int TrackId { get; set; }
        public virtual Track Tracks { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public virtual User Users { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }





    }
}