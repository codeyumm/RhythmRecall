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

        public IHttpActionResult AddTrack(List<Track> tracks)
        {
            // if model is not valid
            // ask christine about this
            if ( !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach(var track in tracks)
            {
                db.Tracks.Add(track);
            }

           
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

            Debug.WriteLine("-----------IN update method --------------------");


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
        [HttpPost]
        [Route("api/TrackData/DeleteTrack/{id}")]
        public IHttpActionResult DeleteTrack(int id)
        {

            // get track of given id
            Track track = db.Tracks.Find(id);

            // check if given id's track exist or not
            if (track == null)
            {
                Debug.WriteLine("------ Track doesn't exist ------");

                return NotFound();
            }

            // remove track from database
            db.Tracks.Remove(track);
            db.SaveChanges();

            return Ok();

        }

        // related method

        // Display tracks of user
        [HttpGet]
        [Route("api/TrackData/DisplaySongs/{id}")]
        public IHttpActionResult DisplaySongs(int id)
        {

            List<TrackList> tracks = db.TrackLists.Where(

                track => track.UserId == id

                ).ToList();


            foreach (var item in tracks)
            {
                Debug.WriteLine("----" + item.Tracks.Title + "------");
            }
            return Ok(tracks);
        }

        // Display tracks of discoverd list of user

        // Display tracks of listen later list of user

        // Add track to discoverd list of user


        // Add track to listen later list of user

        // Add User
        [HttpPost]
        [ResponseType(typeof(User))]
        [Route("api/TrackData/AddUser")]
        public IHttpActionResult AddUser(List<User> users)
        {

  
            Debug.WriteLine("--------------------------------");


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach( var user in users)
            {
                db.Userss.Add(user);
            }

            db.SaveChanges();

            return Ok();
        }


    }
}
