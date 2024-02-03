﻿using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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


        // write a review for a song
        [HttpPost]
        // [Route("api/ReviewData/AddReview/{userId}/{trackId}/{title}/{content}")]
        [Route("api/ReviewData/AddReview")]

        public IHttpActionResult AddReview(Review review)
        {

            // check user exist in database or not
            bool isUserExist = ( db.Userss.Find(review.UserId) != null ) ? true : false;

           if( !isUserExist )
            {
                return BadRequest("Given user is not in database");
            }

            // check track exist in database or not
            bool isInTrackList = (db.Tracks.Find(review.TrackId) != null) ? true : false;

            if( !isInTrackList )
            {
                return BadRequest("Given track is not in database");
            }

            // if user has already written review for that song update it
            Review userReview = db.Reviews.Where(user => user.UserId == review.UserId)
                                          .Where(track => track.TrackId == review.TrackId)
                                          .SingleOrDefault();

           
            if ( userReview != null)
            {
                // we can't add the review that we recieve from parameter because it doen't contain ID of review
                // set new content
                userReview.Title = review.Title;
                userReview.Content = review.Content;

                // update to database
                db.Entry(userReview).State = EntityState.Modified;
      
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return Ok("Review updated");
            }

            // if everything is valid, add review to database
            db.Reviews.Add(review);
            db.SaveChanges();




            return Ok("this api will add a review in database");
        }

    }
}
