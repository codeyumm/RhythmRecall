using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RhythmRecall.Models;
using System.Diagnostics;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;

namespace RhythmRecall.Controllers
{
    public class TrackDataController : ApiController
    {
        // get database context
        public ApplicationDbContext db = new ApplicationDbContext();

        // Display Tracks

        [HttpGet]
        [Route("api/TrackData/ListTracks")]
        public List<TrackDto> ListTracks()
        {

            List<Track> Tracks = db.Tracks.ToList();

            List<TrackDto> TrackDtos = new List<TrackDto>();
            Tracks.ForEach(track =>
            {

                TrackDtos.Add(new TrackDto()
                {
                    Id = track.Id,
                    Title = track.Title,
                    
                }); ;
            });

            return TrackDtos;
        }

        [HttpGet]
        [Route("api/TrackData/TrackLis")]
        public List<TrackListDto> ListTracksss()
        {

            List<TrackList> Tracks = db.TrackLists.ToList();

            List<TrackListDto> TrackDtos = new List<TrackListDto>();

            Tracks.ForEach(track =>
            {

                TrackDtos.Add(new TrackListDto()
                {
                    

                }); ;
            });

            return TrackDtos;
        }

        // Find Track

        // Add Track
        [HttpPost]
        [ResponseType(typeof(Track))]
        [Route("api/TrackData/AddTrack")]

        public IHttpActionResult AddTrack(Track track)
        {
            // if model is not valid
            // ask christine about this
            if ( !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tracks.Add(track);
            db.SaveChanges();

            return Ok();
            
        }

        // Update Track
        [HttpPost]
        [Route("api/TrackData/UpdateTrack/{id}")]

        public IHttpActionResult UpdateTrack(int id, Track track)
        {
            // Model state contains all value from post request
            // and it kind of assign those value with model
            // and if there are any invalid value we can throw some message on webpage
            // Model state is kindof server side validation

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check if id from get request and id of track is same or not

            if (id != track.Id)
            {
                // print messag on console
                Debug.WriteLine("ID mismatche between passed id and passed track id");
                return BadRequest();
            }

            // if everything is valid
            // update track to database
            // Entry() return entity of track
            // then changing the sate of that track to "Modified"

            db.Entry(track).State = EntityState.Modified;

            // save to data and check if there are any error while saving to database

            try
            {
                db.SaveChanges();

            } catch(DbUpdateConcurrencyException)
            {

                throw;

            }

            Debug.WriteLine("Passed try catch block");

            return StatusCode(HttpStatusCode.NoContent);

        }


        // Delete Track

        // related method

        // Display tracks of user

        // Display tracks of discoverd list of user

        // Display tracks of listen later list of user

        // Add track to discoverd list of user

        // Add track to listen later list of user

        // Add User
        [HttpPost]
        [ResponseType(typeof(User))]
        [Route("api/TrackData/AddUser")]
        public IHttpActionResult AddUser(User user)
        {

  
            Debug.WriteLine("--------------------------------");


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Userss.Add(user);
            db.SaveChanges();

            return Ok(user);
        }


    }
}
