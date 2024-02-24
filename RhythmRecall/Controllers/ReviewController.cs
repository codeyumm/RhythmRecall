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

            ViewBag.userId = userId;
            ViewBag.trackId = trackId;


            return View();
        }

        // Controller to add review
        // GET: Review/Add/{userId}/{trackId}/
        public ActionResult Add(int userId, int trackId,Review review)
        {



            Debug.WriteLine("user" + review.UserId);
            Debug.WriteLine("track" + review.TrackId);

         

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

            return Redirect($"Display?userId={review.UserId}");
        }



        // Controller to edit review
        // GET: Review/Edit/{userId}/{reviewId}
        public ActionResult Edit(int userId, int reviewId)
        {

 
            // call FindReview api to get review and pass it to view

            // HttpClient to access all http method
            HttpClient client = new HttpClient();

            // url to call API - api/ReviewData/Find/{reviewId}

            string url = $"{baseUrl}/Find/{reviewId}";

            // response to store result from api
            HttpResponseMessage response = client.GetAsync(url).Result;

            // store response in reviewDto object
            ReviewDto reviewDto = response.Content.ReadAsAsync<ReviewDto>().Result;

            Debug.WriteLine($"------ {reviewDto.Title}");

            // pass user and track id Add controller
            Session["userId"] = userId;
            Session["trackId"] = reviewDto.TrackId;
            // pass review to view to diplay it in form
            return View(reviewDto);
        }


        // Controller to remove review 
        // GET: Review/Remove/{userId}/{reviewId}
        public ActionResult Remove(int userId, int reviewId)
        {

            // client
            HttpClient client = new HttpClient();

            // API url - https://localhost:44387/api/ReviewData/RemoveReview/{userId}/{reviewId}
            string url = $"{baseUrl}RemoveReview/{userId}/{reviewId}";

            // send request on url store result as res
            HttpContent content = new StringContent("");

            // set request content to json
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            bool isReviewDeleted;

            if (response.IsSuccessStatusCode)
            {
                isReviewDeleted = true;
            }
            else
            {
                isReviewDeleted = false;

            }

            TempData["isReviewDeleted"] = isReviewDeleted;

            return RedirectToAction("Display", new {  userId = userId });
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
            string url = $"{baseUrl}ListReviewsForUser/{userId}";

            // Debug.WriteLine("API URL - " + url);

            // response object to store result
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("response : " + response);

            IEnumerable<ReviewDto> reviews = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;


            return View(reviews);
        }

    }
}