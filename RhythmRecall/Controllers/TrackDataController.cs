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


        /// <summary>
        /// Returns all tracks in database
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK) with the following content:
        /// - A list of tracks
        /// </returns>
        /// 
        /// <example>
        /// GET: https://localhost:44387/api/TrackData/ListTracks
        /// Response: [{"Id":1,"Title":"Sinister Flows","ListenLater":null},
        ///             {"Id":2,"Title":"Gravitational Pull","ListenLater":null},...]
        /// </example>

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


        // Find Track

        /// <summary>
        /// Adds a track to database
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl -H "Content-Type:application/json" -d @track.json https://localhost:44387/api/trackdata/addtrack
        /// Response: Ok
        /// </example>


        // Add Track
        [HttpPost]
        [ResponseType(typeof(Track))]
        [Route("api/TrackData/AddTrack")]

        // paramter is List<Track> because I wanted pass list of tracks to add multiple track with one curl request
        // I will change it in final version
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


            Debug.WriteLine("Ramla" + track.Id);

            return Ok(track.Id);
            
        }

        // Update Track

        /// <summary>
        /// Updates a track in database
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl -H "Content-Type:application/json" -d @trackUpdate.json https://localhost:44387/api/TrackData/UpdateTrack/1
        /// Response: Ok
        /// </example>
        /// 


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
        /// <summary>
        /// Deletes a track in database
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl  -d "" https://localhost:44387/api/TrackData/DeletTrack/1
        /// Response: Ok
        /// </example>
        /// 

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
        /// <summary>
        /// Returns list of all songs which are either in listen later list or in discoverd list
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK)
        /// </returns>
        /// <param name="id">2</param>
        /// <example>
        /// POST: curl  -d "" https://localhost:44387/api/TrackData/DeletTrack/1
        /// Response: Ok
        /// </example>
        /// 

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



       


    }
}
