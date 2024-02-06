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


            Debug.WriteLine($"----- {review.UserId} --- user ID");

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


        // to delete review
        [HttpPost]
        [Route("api/ReviewData/RemoveReview/{userId}/{reviewId}")]

        public IHttpActionResult RemoveReview(int userId, int reviewId)
        {

            // check user exist or not
            bool isUserExist = (db.Userss.Find(userId) != null) ? true : false;

            if( !isUserExist )
            {
                return BadRequest("Given user is not in database");
            }


            // check user has any review with given reviewId or not
            Review review = db.Reviews.Where(user => user.UserId == userId)
                                             .Where(r => r.Id == reviewId).SingleOrDefault();

            if( review != null )
            {
                // remove review from database
                db.Reviews.Remove(review);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {

                    return BadRequest("There was some problem while updating database");
                }

                return Ok("Review delted");
            }

            // review doesn't exist
            return BadRequest($"There is no review found with id {reviewId} from user {userId}");
        }



        // to edit review
        [HttpPost]
        [Route("api/ReviewData/EditReview/{userId}/{reviewId}")]

        public IHttpActionResult EditReview(int userId, int reviewId, Review updatedReview)
        {

            // check user exist or not
            bool isUserExist = (db.Userss.Find(userId) != null) ? true : false;

            if( !isUserExist )
            {
                return BadRequest("Given user is not in database");
            }


            // check reviewId and review.id is not mismatched
            if( reviewId != updatedReview.Id)
            {
                return BadRequest("ID Mismatched");

            }

            // check user has any review with given reviewId or not
            Review review = db.Reviews.Where(user => user.UserId == userId)
                                             .Where(r => r.Id == reviewId)
                                             .Where(track => track.TrackId == updatedReview.TrackId).SingleOrDefault();


            if (review != null)
            {

                review.Title = updatedReview.Title;
                review.Content = updatedReview.Content;


                // Was getting some error while using db.Entry(review).State = EntityState.Modified
                db.Reviews.Attach(review);
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();


                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return BadRequest("There was some problem while updating database");
                }

                return Ok("Review updated");
            }


            return BadRequest("There was some problem");
        }


        // find review
        [HttpGet]
        [Route("api/ReviewData/Find/{reviewId}")]

        public IHttpActionResult Find(int reviewId)
        {

            // get review from database based on reivewId
            Review review = db.Reviews.Find(reviewId);

            // check if review exist or not, if exist return the review else send a message
            if( review != null)
            {
                // make object of reviewdto and set value according to dto
                ReviewDto reviewDto = new ReviewDto();

                reviewDto.Id = review.Id;
                reviewDto.Title = review.Title;
                reviewDto.Content = review.Content;
                reviewDto.Username = review.Users.Username;
                reviewDto.TrackTitle = review.Tracks.Title;
                reviewDto.UserId = review.UserId;
                reviewDto.TrackId = review.TrackId;

                Debug.WriteLine(reviewDto.Title);

                return Ok(reviewDto);
            } 
            else
            {
                return BadRequest("No review with given id in database");
            }

        }





        // get all reviews of given user id

        [HttpGet]
        [Route("api/ReviewData/GetUserReviews/{userId}")]

        public IHttpActionResult GetUserReviews(int userId)
        {

            // check user exist or not
            bool isUserExist = (db.Userss.Find(userId) != null) ? true : false;

            if (!isUserExist)
            {
                return BadRequest("Given user is not in database");
            }

            // get reviews of user based on userId
            List<Review> userReviews = db.Reviews.Where(user => user.UserId == userId).ToList();

            // to store multiple dto objects
            List<ReviewDto> userReviewsDto = new List<ReviewDto>() { };

            // assign value to reviewdto and add it to list
            foreach( var review in userReviews)
            {
                userReviewsDto.Add( new ReviewDto() 
                { 
                
                    Id = review.Id,
                    TrackId = review.TrackId,
                    TrackTitle = review.Tracks.Title,
                    UserId = review.UserId,
                    Title = review.Title,
                    Content = review.Content,
                    Username = review.Users.Username
                });

            }

            return Ok(userReviewsDto);
        }


    }
}
