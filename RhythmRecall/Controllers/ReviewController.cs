using RhythmRecall.Models;
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
    public class ReviewController : Controller
    {
        // base url
        private string baseUrl = "https://localhost:44387/api/ReviewData/";


        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // GET: Review
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult New(int userId, int trackId)
        {
            Session.Clear();

            // pass user and track id Add controller
            Session["userId"] = userId;
            Session["trackId"] = trackId;

            return View();
        }


        // Controller to add review
        // GET: Review/Add/{userId}/{trackId}
        public ActionResult Add(Review review)
        {
         
            // get user and track id from session variable
            int userId = (int)Session["userId"];
            int trackId = (int)Session["trackId"];

        

            review.UserId = userId;
            review.TrackId = trackId;

            string jsonpayload = jss.Serialize(review);

            Debug.WriteLine(jsonpayload);

            // HttpClient object to user http methods
            HttpClient client = new HttpClient();

            // set up url tp call api
            string url = $"{baseUrl}AddReview";

            Debug.WriteLine($"url {url}");


            // set content and requst type in header
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            // call api with the url
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            bool isAdded;
            if( response.IsSuccessStatusCode)
            {
                isAdded = true;
            } 
            else
            {
                isAdded = false;
                // Debug.WriteLine("status code not successfull");
            }

            // set isAdded in temp data to access it in view
            TempData["isAdded"] = isAdded;

            return Redirect("Display");
        }



        // Controller to edit review
        // GET: Review/Edit/{userId}/{reviewId}
        public ActionResult Edit(int userId, int reviewId)
        {
            // make search controller

            return View();
        }


        // Controller to remove review 
        // GET: Review/Remove/{userId}/{reviewId}
        public ActionResult Remove()
        {
            return View();
        }

        // Controller to display all reviews by user
        // GET: Review/Display/{userId}

        [HttpGet]

        public ActionResult Display(int userId)
        {
            // this session variable is coming from add review
            // so, I have to set it when I make controller for view reviews
            // get userid from session variable
            

            // client object to use httpclient methods
            HttpClient client = new HttpClient();

            // url of an api to call
            string url = $"{baseUrl}GetUserReviews/{userId}";

            Debug.WriteLine("API URL - " + url);

            // response object to store result
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("response : " + response);

            IEnumerable<ReviewDto> reviews = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;


            return View(reviews);
        }

    }
}