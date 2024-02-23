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


        // --- API's for listen later list 

        // add songs to listen later

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





        /// <summary>
        /// Returns all a list of songs in listen later list
        /// </summary>
        /// 
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all songs in user's listen later list 
        /// </returns>
        /// 
        /// <example>
        /// GET: https://localhost:44387/api/TrackListData/GetListenLaterList/18
        /// 
        /// response: [{"Id":5,"Title":"Maut Ka Safar","Username":"PunkRebel","UserId":18,"TrackId":5,"Artist":"Seedhe Maut"},
        ///             {"Id":6,"Title":"Gravity Waves".....]
        /// </example>

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
                    Username = track.Userss.Username,
                    Artist = track.Tracks.Artist,
                    UserId = track.Userss.Id,
                    TrackId = track.Tracks.Id
                });
            }

            return Ok(listenLaterList);
        }



        /// <summary>
        /// Adds a song to listen later list of user
        /// </summary>
        /// 
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl -H "Content-Type:application/json" -d @listenLater.json https://localhost:44127/api/tracklistdatax/AddToListenLater
        /// response: Ok
        /// </example>


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


            // check if user already have the trak in thier discoverd list or not
            // turnary operator returns true if record exists or false
            bool isInDiscoverdList = (db.TrackLists.Where(track => track.TrackId == trackId)
                                                .Where(user => user.UserId == userId)
                                                .Where(dList => dList.Discovered == 1).Count() == 1) ? true : false;

            // if track exist and it is not in user's listen later list add that song to list
            // else send an error message

            Debug.WriteLine(" is track exist : " + isTrackExist);
            Debug.WriteLine(" is track added : " + isAlreadyAdded);


            // at this point there are four possiblity
            // 1. track doesnt exist in database -> send message to user
            // 2. track exist -> track is already in listen later list -> send message to user
            // 3. track exist -> track is in discoverd list -> update track status in tracklist to listen later = 1 and discoverd = 0
            // 4. track exist -> track is not in both list -> add tracklist to tracklists database with discoverd = 0 and listen later = 1

            if( isTrackExist)
            {
                Debug.WriteLine("Given track is in database");

                // when i'ts a new track entry in list
                if( !isAlreadyAdded && !isInDiscoverdList )
                {
                    TrackList tracklist = new TrackList();
                    tracklist.UserId = userId;
                    tracklist.TrackId = trackId;
                    tracklist.Discovered = 0;
                    tracklist.ListenLater = 1;

                    db.TrackLists.Add(tracklist);
                    db.SaveChanges();

                    Debug.WriteLine("Tried to adding in database");

                }
                else if( isInDiscoverdList ) // if track is in discoverd list
                {
                    // get the track and change the value of discoverd and listen later
                    TrackList tracklist = db.TrackLists.Where(user => user.UserId == userId)
                                                        .Where(track => track.TrackId == trackId).SingleOrDefault();

                    tracklist.Discovered = 0;
                    tracklist.ListenLater = 1;

                    // update in database
                    db.Entry(tracklist).State = EntityState.Modified;
                    db.SaveChanges();

                } else if( isAlreadyAdded)
                {
                    return BadRequest("Track is already in listen later list");
                }

                return BadRequest("There was some error");
            } 
            else
            {
                Debug.Write("There was some error");
                return BadRequest("There was some error");
            }


        }


        /// <summary>
        /// Remove a song to listen later list of user
        /// </summary>
        /// 
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl  -d "" https://localhost:44127/api/TrackListData/removeFromListenLater/1/4
        /// response: User 1 removed trak with id 4 from his list
        /// </example>
        /// 


        // remove song from listen later list
        // check if song exist in listen later list for a particular user or  not if exist move further
        // else send an error message
        // if req is valid remove song from listen later list

        [HttpPost]
        [Route("api/TrackListData/removeFromListenLater/{userId}/{trackId}")]

        public IHttpActionResult RemoveFromListenLater(int userId, int trackId)
        {
            Debug.WriteLine("--- removing from list ----");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // QUERY -> Find a record which has userId, trackId and it should have listen later == 1
            // SingleOrDefault - either return one element or default vaue if result is null
            // here we are sure that we will recieve either one row or null so we can use SingleOrDefault

                                                           
            TrackList tracklist = db.TrackLists.Where(user => user.UserId == userId)
                                                .Where(track => track.TrackId == trackId)
                                                .Where(listenLater => listenLater.ListenLater == 1).SingleOrDefault();

           

            if ( tracklist != null  )
            {

                Debug.WriteLine("remove from list");

                // remove from list
                db.TrackLists.Remove(tracklist);
                db.SaveChanges();

            } 
            else
            {
                // send error message
                Debug.WriteLine("can't remove from list");

                return BadRequest("Can't remove from list");

            }

            return Ok($" User {userId} removed track with id {trackId}  his list ");
        }







        // --- API's for discoverd list





        /// <summary>
        /// Returns all a list of songs in discoverd list
        /// </summary>
        /// 
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all songs in user's listen later list 
        /// </returns>
        /// 
        /// <example>
        /// GET: https://localhost:44387/api/TrackListData/GetDiscoverdList/6
        /// 
        /// response: [{"Id":14,"Title":"Galactic Groove",
        ///                 "Username":"IndieExplorer","UserId":6,"TrackId":14,"Artist":"SUPERVILLAIN"},
        ///                 {"Id":15,"Title":"Desi Swagger".....]
        /// </example>

        // get list of discoverd song

        [HttpGet]
        [ResponseType(typeof(TrackList))]
        [Route("api/TrackListData/GetDiscoverdList/{userId}")]

        public IHttpActionResult GetDiscoverdList(int userId)
        {

 
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);

            }

  
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
                    Username = track.Userss.Username,
                    Artist = track.Tracks.Artist,
                    UserId = track.Userss.Id,
                    TrackId = track.Tracks.Id

                });
            }


            return Ok(dsicovedList);
        }


        /// <summary>
        /// Adds a song to discoverd list of user
        /// </summary>
        /// 
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl -H "Content-Type:application/json" -d @listenLater.json https://localhost:44127/api/tracklistdatax/AddToDiscoverdList
        /// response: Ok
        /// </example>

        // add song to discoverd list
        // we will need trackId, UserId
        // before addig we have to check that
        // if song and user exist or not and if user already has that song in discoverd list
        // send some error message . .
        // else
        // add that song to discoverd list

        [HttpPost]
        [Route("api/TrackListData/AddToDiscoverdList/{userId}/{trackId}")]

        public IHttpActionResult AddToDiscoverdList(int userId, int trackId)
        {

            // check track with trackId from request exist or not
            // get track with trackId
            // if the query returns 1 track exist if it returns 0 track doesn't exist
                                              // turnary operator returns true if record exists or false
            bool isTrackExist = (db.Tracks.Where(t => t.Id == trackId).Count() == 1) ? true : false;


            // check if user already have the trak in thier discoverd list or not
                                            // turnary operator returns true if record exists or false
            bool isInDiscoverdList = (db.TrackLists.Where(track => track.TrackId == trackId)
                                                .Where(user => user.UserId == userId)
                                                .Where(dList => dList.Discovered == 1).Count() == 1) ? true : false;

            // check if user already have the trak in thier listen later list or not
                                            // turnary operator returns true if record exists or false
            bool isInListenLaterList = (db.TrackLists.Where(track => track.TrackId == trackId)
                                                .Where(user => user.UserId == userId)
                                                .Where(lList => lList.ListenLater == 1).Count() == 1) ? true : false;


            Debug.WriteLine( $"User want to add {trackId} track, \n is it in Discoverdlist? {isInDiscoverdList} \n is It in listen later list? {isInListenLaterList} \n ----" );


            // at this point there are four possiblity
            // 1. track doesnt exist in database -> send message to user
            // 2. track exist -> track is already in discoverd list -> send message to user
            // 3. track exist -> track is in listen later list -> update track status in tracklist to listen later = 0 and discoverd = 1
            // 4. track exist -> track is not in both list -> add tracklist to tracklists database with discoverd = 1 and listen later = 0

            if (isTrackExist)
            {

                Debug.WriteLine("Given track is in Track database");

                if (isInListenLaterList) {

                    TrackList tracklist = db.TrackLists.Where(user => user.UserId == userId).Where( track => track.TrackId == trackId).SingleOrDefault();

                    tracklist.ListenLater = 0;
                    tracklist.Discovered = 1;
                    db.Entry(tracklist).State = EntityState.Modified;
                    db.SaveChanges();

                    return Ok("Song added to discoverd list");

                } 
                else if(isInDiscoverdList)
                {

                    return Ok("Song is already in discoverd list");

                } 
                else if( !isInDiscoverdList && !isInListenLaterList)
                {

                    TrackList tracklist = new TrackList();
                    tracklist.TrackId = trackId;
                    tracklist.UserId = userId;
                    tracklist.ListenLater = 0;
                    tracklist.Discovered = 1;

                    db.TrackLists.Add(tracklist);
                    db.SaveChanges();

                    return Ok("Song added to discoverd list");
                }

                return BadRequest("There were some error");

            }
            else
            {
                Debug.WriteLine("There is some error");

                return BadRequest("There were some error");
            }

        }



        /// <summary>
        /// Remove a song to discoverd list of user
        /// </summary>
        /// 
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl  -d "" https://localhost:44127/api/TrackListData/removeFromListenLater/1/4
        /// response: User 1 removed trak with id 4 from his list
        /// </example>
        /// 

        // remove song from discoverd list

        [HttpPost]
        [Route("api/TrackListData/removeFromDiscoverd/{userId}/{trackId}")]

        public IHttpActionResult RemoveFromDiscoverd(int userId, int trackId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // QUERY -> Find a record which has userId, trackId and it should have discoverd == 1
            // SingleOrDefault - either return one element or default vaue if result is null
            // here we are sure that we will recieve either one row or null so we can use SingleOrDefault


            TrackList tracklist = db.TrackLists.Where(user => user.UserId == userId)
                                                .Where(track => track.TrackId == trackId)
                                                .Where(discoverd => discoverd.Discovered == 1).SingleOrDefault();

            if (tracklist != null)
            {

                Debug.WriteLine("remove from list");

                // remove from list
                db.TrackLists.Remove(tracklist);
                db.SaveChanges();

            }
            else
            {
                // send error message
                Debug.WriteLine("can't remove from list");

                return BadRequest("Can't remove from list");

            }

            return Ok($" User {userId} removed track with id {trackId} from his list");
        }


    }
}
