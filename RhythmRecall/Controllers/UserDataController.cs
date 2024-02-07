using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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
    }
}
