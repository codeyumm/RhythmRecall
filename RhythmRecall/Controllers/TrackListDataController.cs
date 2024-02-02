using RhythmRecall.Migrations;
using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Description;
using Track = RhythmRecall.Models.Track;

namespace RhythmRecall.Controllers
{
    public class TrackListDataController : ApiController
    {
        // get database context
        public ApplicationDbContext db = new ApplicationDbContext();

        // define route here

        // add song to listen later

        [HttpPost]
        [Route("api/TrackListDatax/AddToListenLater")]

        public IHttpActionResult AddToListenLater(List<TrackList> tracklists)
        {

            if( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            foreach(var tracklist in tracklists)
            {
                db.TrackLists.Add(tracklist);
            }
  
            db.SaveChanges();

            return Ok();
        }



        // get list of listen later song

        [HttpGet]
        [ResponseType(typeof(TrackList))]
        [Route("api/TrackListData/GetListenLaterList/{userId}")]

        public IHttpActionResult GetListenLaterList(int userId)
        {

            // check if model state is valid or not
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);

            }

            // if data is valid
            // get listen later list of user accordin to userId and listen later should be true and discovered should be false
            List<TrackList> tracklist =  db.TrackLists.Where(tl => tl.UserId == userId)
                                                    .Where (tl => tl.ListenLater == 1)
                                                    .Where (tl => tl.Discovered == 0)
                                                    .ToList();


            // if we send tracklist it will send lot of data, which is not useful to user
            // to reduce the load using dto
            // create tracklist dto object
            List<TrackListDto> listenLaterList = new List<TrackListDto> { };



            // iterate through each object of tracklist
            foreach( var track in tracklist)
            {

                // get the value according to dto
                // append object to listenlaterlist
                listenLaterList.Add(new TrackListDto
                {
                    Id = track.TrackId,
                    Title = track.Tracks.Title,
                    Username = track.Userss.Username

                });
            }


            return Ok(listenLaterList);
        }


        // add song to listen later list
        // we will need trackId, UserId
        // before addig we have to check that
        // if song and user exist or not and if user already has that song in listen later list
        // send some error message . .
        // else
        // add that song to listen later list

        [HttpPost]
        [Route("api/TrackListData/AddToListenLaterList/{userId}/{trackId}")]

        public IHttpActionResult AddToLIstenLaterList(int userId, int trackId)
        {

            // check track with trackId from request exist or not
            // get track with trackId
            // if the query returns 1 track exist if it returns 0 track doesn't exist
            bool isTrackExist =   ( db.Tracks.Where(t => t.Id == trackId).Count() == 1 ) ? true : false;

            // check if user already have the trak in thier listen later list or not
            bool isAlreadyAdded = ( db.TrackLists.Where(track => track.TrackId == trackId).
                                                Where( user => user.UserId == userId).Count() == 1 ) ? true : false;

            // if track exist and it not in user's listen later list
            // add that song to list

            if( isTrackExist && !isAlreadyAdded)
            {
                Debug.WriteLine("You are good to go");
            } else
            {
                Debug.WriteLine("There is some error");
            }


            return Ok("To add song in listen later list");
        }



















        // get list of discoverd song

        [HttpGet]
        [ResponseType(typeof(TrackList))]
        [Route("api/TrackListData/GetDiscoverdList/{userId}")]

        public IHttpActionResult GetDiscoverdList(int userId)
        {

            // check if model state is valid or not
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);

            }

            // if data is valid
            // get listen later list of user accordin to userId and listen later should be false and discovered should be true

            List<TrackList> tracklist = db.TrackLists.Where(tl => tl.UserId == userId)
                                                    .Where(tl => tl.ListenLater == 0)
                                                    .Where(tl => tl.Discovered == 1)
                                                    .ToList();


            // if we send tracklist it will send lot of data, which is not useful to user
            // to reduce the load using dto
            // create tracklist dto object
            List<TrackListDto> dsicovedList = new List<TrackListDto> { };



            // iterate through each object of tracklist
            foreach (var track in tracklist)
            {

                // get the value according to dto
                // append object to listenlaterlist
                dsicovedList.Add(new TrackListDto
                {
                    Id = track.TrackId,

                    Title = track.Tracks.Title,

                    Username = track.Userss.Username

                });
            }


            return Ok(dsicovedList);
        }


    }
}
