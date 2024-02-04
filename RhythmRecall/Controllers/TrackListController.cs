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

        [HttpGet]
        public ActionResult ListenLater(int id)
        {
            
            HttpClient client = new HttpClient();

            string url = $"https://localhost:44387/api/TrackListData/GetListenLaterList/{id}";

            HttpResponseMessage res = client.GetAsync(url).Result;



            IEnumerable<TrackListDto> tracks = res.Content.ReadAsAsync<IEnumerable<TrackListDto>>().Result;

            

            foreach( var t in tracks)
            {
                Debug.WriteLine($"--- {t.Title}");
            }

            return View(tracks);
        }
        
    }
}