using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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

    }
}
