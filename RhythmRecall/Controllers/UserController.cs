using RhythmRecall.Models;
using RhythmRecall.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RhythmRecall.Controllers
{
    public class UserController : Controller
    {
        string baseUrl = "https://localhost:44387/api/UserData/";

        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckProfile(string username)
        {

            // HttpClient object to use http method
            HttpClient client = new HttpClient();

            // url to call the api
            string url = $"{baseUrl}/CheckUsername/{username}";

            Debug.WriteLine(url);

            HttpResponseMessage response = client.GetAsync(url).Result;

            UserDto user = response.Content.ReadAsAsync<UserDto>().Result;

            if( response.IsSuccessStatusCode)
            {
                Debug.WriteLine("User found");

                return RedirectToAction("/DisplayProfile", new { id = user.Id });
            } else
            {
                Debug.WriteLine("User NOT found");

            }

            return View();
        }

        // using post method here because we passing username and password
        // POST: User/ValidateUser

        public ActionResult ValidateUser(User user)
        {

            // check username and password
            Debug.WriteLine(user.Username);
            Debug.WriteLine(user.Password);

            // validate username and password
            HttpClient client = new HttpClient();

            // api to call api
            string url = $"{baseUrl}/Validate";

            string jsonpayload = jss.Serialize(user);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";


            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if( response.IsSuccessStatusCode)
            {
                User resUser = response.Content.ReadAsAsync<User>().Result;

                Session.Clear();

                ViewBag.userId = resUser.Id;
                Session["userId"] = resUser.Id;
   
                return Redirect("/Home/Search");
            }

            Debug.WriteLine("Hello");

            return Redirect("/Home/Index");
        }



        // GET: User/DisplayProfile/id

        public ActionResult DisplayProfile(int id)
        {

            // In profile we need data from 3 models.
            // 1. User
            // 2. TrackList
            // 3. Review
            // Using view models for this
            DetailsUser ViewModel = new DetailsUser();


            //client object for HttpClien to use http methods
            HttpClient client = new HttpClient();


            // for user info

            // url to call the api 
            string url = $"{baseUrl}GetProfileInfo/{id}";


            Debug.WriteLine("--- URL" + url);

            // response to store the result from api
            HttpResponseMessage response = client.GetAsync(url).Result;

            // store the response in userDto object
            UserDto user = response.Content.ReadAsAsync<UserDto>().Result;

           
          

            Debug.WriteLine("--- Response" + user.Username);

            // set user to view model
            ViewModel.SelectedUser = user;

            

            // for listen later list
       
            // url to call the api to get track list
            url = $"https://localhost:44387/api/TrackListData/GetListenLaterList/{id}";

            // get response from api
            response = client.GetAsync(url).Result;

            // store respone in tracklist object
            IEnumerable<TrackListDto> listenLaterList = response.Content.ReadAsAsync<IEnumerable<TrackListDto>>().Result;

            ViewModel.UserListenLaterList = listenLaterList;

            // for discoverd list

            // url to call the api to get track list
            url = $"https://localhost:44387/api/TrackListData/GetDiscoverdList/{id}";

            // get response from api
            response = client.GetAsync(url).Result;

            // store respone in tracklist object
            IEnumerable<TrackListDto> discoverdList = response.Content.ReadAsAsync<IEnumerable<TrackListDto>>().Result;

            ViewModel.UserDiscoverdList = discoverdList;



            // for reviews

            // url to call the api to get track list
            url = $"https://localhost:44387/api/ReviewData/GetUserReviews/{id}";

            // get response from api
            response = client.GetAsync(url).Result;

            // store respone in tracklist object
            IEnumerable<ReviewDto> reviews = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;

            ViewModel.UserReviews = reviews;

            // pass ViewModel to view
            return View(ViewModel);
        }


        // GET: User/IntrestedUserForLitenLater
        public ActionResult IntrestedUserForLitenLater(int userId, int trackId)
        {

            // HttpClient object to use http method
            HttpClient client = new HttpClient();

            // url to call the api
            string url = $"{baseUrl}findIntrestedUserForListenLater/{trackId}";

            Debug.WriteLine(url);

            HttpResponseMessage response = client.GetAsync(url).Result;

            List<TrackListDto> tracklist = new List<TrackListDto> { };
            // check the response
            if ( response.IsSuccessStatusCode )
            {

                // get the response in tracklistdto List
                tracklist = response.Content.ReadAsAsync<List<TrackListDto>>().Result;


            } else
            {
                
            } 

            ViewBag.userId = userId;


            return View(tracklist);
        }


        // GET: User/IntrestedUserForDiscoverd
        public ActionResult IntrestedUserForDiscoverd(int userId, int trackId)
        {

            // HttpClient object to use http method
            HttpClient client = new HttpClient();

            // url to call the api
            string url = $"{baseUrl}findIntrestedUserForDiscoverd/{userId}/{trackId}";

            Debug.WriteLine(url);

            HttpResponseMessage response = client.GetAsync(url).Result;

            List<TrackListDto> tracklist = new List<TrackListDto> { };
            // check the response
            if (response.IsSuccessStatusCode)
            {

                // get the response in tracklistdto List
                tracklist = response.Content.ReadAsAsync<List<TrackListDto>>().Result;


            }
            else
            {

            }

            ViewBag.userId = userId;

            return View(tracklist);
        }
    }
}