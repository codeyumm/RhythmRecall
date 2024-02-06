using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
// using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RhythmRecall.Controllers
{
    public class TrackListController : Controller
    {
        // base url
        private string baseUrl = "https://localhost:44387/api/TrackListData/";

        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // GET: TrackList
        public ActionResult Index()
        {
            return View();
        }


        // GET: TrackList/ListenLater/{id}
        // here id is id of user

        [HttpGet]
        public ActionResult ListenLater(int id)
        {
            
            // we need object of HttpClient to use http methods
            HttpClient client = new HttpClient();

            // setting up url to call api
            string url = $"https://localhost:44387/api/TrackListData/GetListenLaterList/{id}";

            // send request on url store result as res
            HttpResponseMessage res = client.GetAsync(url).Result;

            // as we are getting response as list of tracklistdto saving it in list of tracklistdto
            IEnumerable<TrackListDto> tracks = res.Content.ReadAsAsync<IEnumerable<TrackListDto>>().Result;

            // when redirecting from RemoveFromListenLater we want to display a message

            bool isTrackDeleted =  TempData["isTrackDeleted"] as bool? ?? false;
            bool isAddedToDiscoverd = TempData["isAddedToDiscoverd"] as bool? ?? false;
            bool isAddedToListenLater = TempData["isAddedToListenLater"] as bool? ?? false;

            ViewBag.isTrackDeleted = isTrackDeleted;
            ViewBag.isAddedToDiscoverd = isAddedToDiscoverd;
            ViewBag.isAddedToListenLater = isAddedToListenLater;

            // send list of tracklistdto to view to display on webpage
            return View(tracks);
        }
        




        // GET: TrackList/AddToListenLater/userId/TrackId
        [HttpPost]

        public ActionResult AddToListenLater(int userId, int trackId)
        {
            Debug.WriteLine(userId + " ---- ohoohoo-- --- -- -" + trackId);

            // object of httpclient to use http methods
            HttpClient client = new HttpClient();

            // url of api
            string url = $"{baseUrl}AddToListenLaterList/{userId}/{trackId}";

            Debug.WriteLine("--- URL " + url);

            // send request on url store result as res
            HttpContent content = new StringContent("");

            // set request content to json
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            bool isAddedToListenLater;
            // check if response was succesfull or not
            if (response.IsSuccessStatusCode)
            {
                isAddedToListenLater = true;
            }
            else
            {
                isAddedToListenLater = false;
            }


            // store var in temp data to send it in another controller
            TempData["isAddedToListenLater"] = isAddedToListenLater;

            return Redirect($"Discoverd/{userId}");

        }







        // GET: TrackList/RemoveFromListenLater/userId/trackId

        [HttpPost]

        public ActionResult RemoveFromListenLater(int userId, int trackId)
        {

            // we need object of HttpClient to use http methods
            HttpClient client = new HttpClient();

            // setting up url to call api
            string url = $"{baseUrl}RemoveFromListenLater/{userId}/{trackId}";

            // send request on url store result as res
            HttpContent content = new StringContent("");

            // set request content to json
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;


            bool isTrackDeleted;
            // check if response was succesfull or not
            if (response.IsSuccessStatusCode)
            {
                isTrackDeleted = true;
            } else
            {
                isTrackDeleted = false;
            }


            // store var in temp data to send it in another controller
            TempData["isTrackDeleted"] = isTrackDeleted;

            return Redirect($"ListenLater/{userId}");
        }


        // ---------------------------------
        //  Controllers for discoverd list
        // --------------------------------_



        // GET: TrackList/DiscoverdList/{userId}
        // to get a list of discoverd song
        [HttpGet]

        public ActionResult Discoverd(int id)
        {
            // we need object of HttpClient to use http methods
            HttpClient client = new HttpClient();

            // setting up url to call api
            string url = $"{baseUrl}/GetDiscoverdList/{id}";

            // send request on url store result as res
            HttpResponseMessage res = client.GetAsync(url).Result;

            // as we are getting response as list of tracklistdto saving it in list of tracklistdto
            IEnumerable<TrackListDto> tracks = res.Content.ReadAsAsync<IEnumerable<TrackListDto>>().Result;

            // when redirecting from RemoveFromListenLater we want to display a message

            bool isTrackDeleted = TempData["isTrackDeleted"] as bool? ?? false;
            bool isAddedToDiscoverd = TempData["isAddedToDiscoverd"] as bool? ?? false;

            ViewBag.isTrackDeleted = isTrackDeleted;
            ViewBag.isAddedToDiscoverd = isAddedToDiscoverd;

            // send list of tracklistdto to view to display on webpage
            return View(tracks);

        }


        // GET: TrackList/AddToDiscoverdList/{userId}/{trackId}

        public ActionResult AddToDiscoverdList(int userId, int trackId)
        {
            // object of httpclient to use http methods
            HttpClient client = new HttpClient();

            // url of api
            string url = $"{baseUrl}AddToDiscoverdList/{userId}/{trackId}";

            Debug.WriteLine("--- URL " + url);

            // send request on url store result as res
            HttpContent content = new StringContent("");

            // set request content to json
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            bool isAddedToDiscoverd;
            // check if response was succesfull or not
            if (response.IsSuccessStatusCode)
            {
                isAddedToDiscoverd = true;
            }
            else
            {
                isAddedToDiscoverd = false;
            }



            // store var in temp data to send it in another controller
            TempData["isAddedToDiscoverd"] = isAddedToDiscoverd;

            return Redirect($"ListenLater/{userId}");

        }

        [HttpPost]
        public ActionResult RemoveFromDiscoverd(int userId, int trackId)
        {

            // get httplcient object
            HttpClient client = new HttpClient();

            // set up url of the api
            string url = $"{baseUrl}/RemoveFromDiscoverd/{userId}/{trackId}";

            Debug.WriteLine("URL to call remove from discoverd : " + url);

            // send request to call api method equivalant to -d in curl
            HttpContent content = new StringContent("");


            // set request content to json
            content.Headers.ContentType.MediaType = "application/json";

            // make a reuest to url and store the res
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            bool isTrackDeleted;

            if( response.IsSuccessStatusCode)
            {
                isTrackDeleted = true;
            } else
            {
                isTrackDeleted = false;
            }

            TempData["isTrackDeleted"] = isTrackDeleted;

            return Redirect($"Discoverd/{userId}");
        }












    }
}