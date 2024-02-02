using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RhythmRecall.Models
{
    public class User
    {
        /*    Table User {
              UserID int [pk]
              Username varchar
              Email varchar
              Password varchar } 
        */

        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        // TrackList can have many users
        public ICollection<TrackList> TrackLists { get; set; }

        // Reviews can have many users
        public ICollection<Review> Reviews { get; set; }


    }
}