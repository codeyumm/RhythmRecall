using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace RhythmRecall.Controllers
{
    public class UserDataController : ApiController
    {
        
        ApplicationDbContext db = new ApplicationDbContext();


        // Add User
        [HttpPost]
        [ResponseType(typeof(User))]
        [Route("api/UserData/Add")]
        public IHttpActionResult Add(List<User> users)
        {


            Debug.WriteLine("--------------------------------");


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var user in users)
            {
                db.Userss.Add(user);
            }

            db.SaveChanges();

            return Ok();
        }





        [HttpGet]
        [Route("api/UserData/GetProfileInfo/{id}")] // here id is user id

        public IHttpActionResult GetProfileInfo(int id)
        {

            // check if user exist or not
            // get all info from users table except password

            User user = db.Userss.Find(id);





            if( user != null)
            {
                // create user dto object
                UserDto userDto = new UserDto();

                // set value in userDto from user object
                userDto.Id = id;
                userDto.Username = user.Username;
                userDto.Email = user.Email;
                userDto.Firstname = user.Firstname;
                userDto.Lastname = user.Lastname;


                // user exist and return userDto object as a response
                return Ok(userDto);
            }
            else
            {
                return BadRequest("No user found in database");
            }

            
        }

        [HttpGet]
        [Route("api/UserData/findIntrestedUserForListenLater/{id}")]
        
        public IHttpActionResult findIntrestedUserForListenLater(int id)
        {
            // check if track with given id exist or not

            Track track = db.Tracks.Find(id);

            // send badrequst if track is not found
            if ( track == null )
            {
                return BadRequest("track is not in database");
            }

            // find all users who have given track in thier listen later list
            List<TrackList> tracklist = db.TrackLists.Where(t => t.TrackId == id)
                                                  .Where(listenlater => listenlater.ListenLater == 1)
                                                  .ToList();

            List<TrackListDto> tracklistDto = new List<TrackListDto> { };


            // iterate through each tracklist row and set it to tracklistdto value
            // append new tracklistdto object in tracklistdto list
            foreach(var tl in tracklist)
            {

                tracklistDto.Add(new TrackListDto
                {
                    Id = tl.id,
                    Title = tl.Tracks.Title,
                    Username = tl.Userss.Username,
                    UserId = tl.UserId,
                    TrackId = tl.TrackId,
                    Artist = tl.Tracks.Artist

                });
            }


            return Ok(tracklistDto);
        }



        
        [HttpGet]
        [Route("api/UserData/findIntrestedUserForDiscoverd/{userId}/{trackId}")]

        public IHttpActionResult findIntrestedUserForDiscoverd(int userId, int trackId)
        {
            // check if track with given id exist or not

            Track track = db.Tracks.Find(trackId);

            // send badrequst if track is not found
            if (track == null)
            {
                return BadRequest("track is not in database");
            }

            // find all users who have given track in thier listen later list
            List<TrackList> tracklist = db.TrackLists.Where(t => t.TrackId == trackId)
                                                  .Where(discoverd => discoverd.Discovered == 1)
                                                  .ToList();

            List<TrackListDto> tracklistDto = new List<TrackListDto> { };


            // iterate through each tracklist row and set it to tracklistdto value
            // append new tracklistdto object in tracklistdto list
            foreach (var tl in tracklist)
            {

                if( tl.UserId == userId ) { continue; }

                tracklistDto.Add(new TrackListDto
                {
                    Id = tl.id,
                    Title = tl.Tracks.Title,
                    Username = tl.Userss.Username,
                    UserId = tl.UserId,
                    TrackId = tl.TrackId,
                    Artist = tl.Tracks.Artist

                });
            }

            return Ok(tracklistDto);
        }

        [HttpPost]
        [Route("api/UserData/Remove/{id}")]

        public IHttpActionResult Remove(int id)
        {

            // check if user exist or not
            User user = db.Userss.Find(id);
           
            if( user != null)
            {
                // remove user from database
                db.Userss.Remove(user);
                db.SaveChanges();

                return Ok("User deleted from database.");
            }
            else
            {
                return BadRequest("User not found.");
            }

        }



        // get user info from username
        [HttpGet]
        [Route("api/UserData/CheckUsername/{id}")] // here id is user id

        public IHttpActionResult CheckUsername(string id)
        {

            // check if user exist or not
            // get all info from users table except password

            User user = db.Userss.Where( u => u.Username == id).FirstOrDefault();


  
            if (user != null)
            {
                // create user dto object
                UserDto userDto = new UserDto();

                // set value in userDto from user object
                userDto.Id = user.Id;
                userDto.Username = user.Username;
                userDto.Email = user.Email;
                userDto.Firstname = user.Firstname;
                userDto.Lastname = user.Lastname;


                // user exist and return userDto object as a response
                return Ok(userDto);
            }
            else
            {
                return BadRequest("No user found in database");
            }


        }


        // get all usernames from database
        // get user info from username
        [HttpGet]
        [Route("api/UserData/getUsernames")] // here id is user id

        public IHttpActionResult getUsernames()
        {

            // check if user exist or not
            // get all info from users table except password

            List<User> user = db.Userss.ToList();

            List<UserDto> userDto = new List<UserDto> { };




            if (user != null)
            {

                foreach(var u in user)
                {
                    userDto.Add( new UserDto
                    {
                        // set value in userDto from user object
                        Id = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        Firstname = u.Firstname,
                        Lastname = u.Lastname,
                });
                }
               


                // user exist and return userDto object as a response
                return Ok(userDto);
            }
            else
            {
                return BadRequest("No user found in database");
            }


        }
    }
}
