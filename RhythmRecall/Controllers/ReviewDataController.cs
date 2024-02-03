using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RhythmRecall.Controllers
{

    public class ReviewDataController : ApiController
    {
        // get database context
        public ApplicationDbContext db = new ApplicationDbContext();


        // get all reviews of a song based ons songid
        [HttpGet]
        [Route("api/ReviewData/getReviews/{trackId}")]

        public IHttpActionResult GetReviews(int trackId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check track with given trackId exist or not
            Track track = db.Tracks.Find(trackId);

            if (track != null)
            {
                Debug.WriteLine("Track found");

                // get all reviews from database of the song
                List<Review> reviews = db.Reviews.Where(t => (t.TrackId == trackId) ).ToList();

                return Ok(reviews);
            } 
            else
            {
                Debug.WriteLine("Track is not in database");

                return BadRequest("Track is not in database");
            }


        }

    }
}
