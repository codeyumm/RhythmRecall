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
        private string baseUrl = "https://localhost:44387/api/Review/";


        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // GET: Review
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult New(int userId, int trackId)
        {

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

            Review newReview = new Review();
            newReview.UserId = userId;
            newReview.TrackId = trackId;
            newReview.Title = review.Title;
            newReview.Content = review.Content;

            string jsonpayload = jss.Serialize(newReview);

            Debug.WriteLine(jsonpayload);

            // HttpClient object to user http methods
            HttpClient client = new HttpClient();

            // set up url tp call api
            string url = $"{baseUrl}Add/userId";

            // set content and requst type in header
            HttpContent content = new StringContent("");
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
            }

            // set isAdded in temp data to access it in view
            TempData["isAdded"] = isAdded;

            return Redirect($"~/TrackList/ListenLater/{userId}");
        }



        // Controller to edit review
        // GET: Review/Edit/{userId}/{reviewId}
        public ActionResult Edit()
        {
            return View();
        }


        // Controller to remove review 
        // GET: Review/Remove/{userId}/{reviewId}
        public ActionResult Remove()
        {
            return View();
        }

    }
}