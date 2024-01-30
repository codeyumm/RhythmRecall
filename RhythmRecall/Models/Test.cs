using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RhythmRecall.Models
{
    public class Test
    {
        /*       Table User {
              UserID int [pk]
              Username varchar
              Email varchar
              Password varchar } 
        */

        [Key]
        public int Id { get; set; }

        public string Username { get; set; }


    }
}