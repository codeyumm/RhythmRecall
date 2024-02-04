using RhythmRecall.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RhythmRecall.Controllers
{
    public class TrackListController : Controller
    {
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

            // send list of tracklistdto to view to display on webpage
            return View(tracks);
        }
        
    }
}