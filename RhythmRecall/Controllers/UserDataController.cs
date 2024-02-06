using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RhythmRecall.Controllers
{
    public class UserDataController : ApiController
    {
        
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/UserData/GetProfileInfo/{id}")] // here id is user id

        public IHttpActionResult GetProfileInfo(int id)
        {

            // check if user exist or not
            // get all info from users table except password

            User user = db.Userss.Find(id);

            // create user dto object
            UserDto userDto = new UserDto();
            
            // set value in userDto from user object
            userDto.Id = id;
            userDto.Username = user.Username;
            userDto.Email = user.Email;



            if( user != null)
            {

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
