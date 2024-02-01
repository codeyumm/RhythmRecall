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
