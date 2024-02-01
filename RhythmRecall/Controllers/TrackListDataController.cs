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
            // get listen later list of user accordin to userId

            List<TrackList> tracklist =  db.TrackLists.Where(

                    tl => tl.UserId == userId
            ).ToList();

            Debug.WriteLine("------" + tracklist.GetType() + "------");


            foreach (var track in tracklist)
            {

                Debug.WriteLine("------" + track.Userss.Username + "------");

                Debug.WriteLine("------" + track.Tracks.Title + "------");
            }


            return Ok(tracklist);
        }


    }
}
