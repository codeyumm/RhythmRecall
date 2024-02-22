using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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


        /// <summary>
        /// Adds a user to database
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK)
        /// </returns>
        /// 
        /// <example>
        /// POST: curl -H "Content-Type:application/json" -d @user.json https://localhost:44387/api/UserData/add
        /// Response: Ok
        /// </example>

        [HttpPost]
        [ResponseType(typeof(User))]
        [Route("api/UserData/Add")]
        // paramter is List<Track> because I wanted pass list of tracks to add multiple track with one curl request
        // I will change it in final version
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




        /// <summary>
        /// Return all information of a user based on user id
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK) with the following content:
        /// - information about user
        /// </returns>
        /// 
        /// <param name="id">id of user</param>
        /// 
        /// <example>
        /// GET: https://localhost:44387/api/UserData/GetProfileInfo/6
        /// 
        /// Response:{"Id":6,"Username":"IndieExplorer","Email":"indie@example.com","Firstname":"Isabella","Lastname":"Anderson"}
        /// </example>

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



        /// <summary>
        /// Returns list of user who have song in thier listen later list
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK) with the following content:
        /// - list of user
        /// </returns>
        /// 
        /// <param name="id">id of track</param>
        /// 
        /// <example>
        /// GET: https://localhost:44387/api/UserData/findIntrestedUserForListenLater/12
        /// 
        /// Response:[{"Id":10,"Title":"Rap Resurrection","Username":"PunkRebel","UserId":18,"TrackId":12,"Artist":"Raftaar"},
        /// {"Id":30,"Title":"Rap Resurrection","Username":"RockNRoller","UserId":3,"TrackId":12,"Artist":"Raftaar"}]
        /// </example>




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


        /// <summary>
        /// Returns list of user who have song in thier discoverd list
        /// </summary>
        /// 
        /// <returns>
        /// HTTP 200 (OK) with the following content:
        /// - list of user
        /// </returns>
        /// 
        /// <param name="id">id of track</param>
        /// <param name="userId">id of user</param>
        /// <example>
        /// GET: https://localhost:44387/api/UserData/findIntrestedUserForDiscoverd/6/18
        /// 
        /// Response:[{"Id":16,"Title":"Anti-Gravity Anthem","Username":"MetalHead","UserId":12,"TrackId":18,"Artist":"SUPERVILLAIN"},{"Id":39,"Title":"Anti-Gravity Anthem","Username":"EDMAddict","UserId":7,"TrackId":18,"Artist":"SUPERVILLAIN"}]
        /// </example>


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

        // to validate username and password
        [HttpPost]
        [Route("api/UserData/Validate")]
        public IHttpActionResult Validate(User user)
        {
            Debug.WriteLine(user.Username);
            Debug.WriteLine(user.Password);

            // returns true or false if user exist or not

             bool isUserExist = (db.Userss.Where( u => u.Username == user.Username)
                                    .FirstOrDefault() == null) ? false : true;

            // Debug.WriteLine(isUserExist + "isUserExists");

            if ( isUserExist)
            {
                // validate user
                User validatedUser = db.Userss.Where(u => u.Username == u.Username)
                                               .Where(u => u.Password == u.Password).FirstOrDefault();
                if( validatedUser!= null ) 
                {
                    return Ok(validatedUser);

                }

                return BadRequest("Wrong password");

            } 
            else
            {
                // return with a message
                return BadRequest("User not found");
            }

           
        }
    }
}
