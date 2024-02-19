using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using RhythmRecall.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace RhythmRecall.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            HttpClient client = new HttpClient();

            string url = "https://localhost:44387/api/UserData/getUsernames";

            HttpResponseMessage response = client.GetAsync(url).Result;

            List <UserDto> usernames = response.Content.ReadAsAsync<List<UserDto>>().Result;

            return View(usernames);
        }

        public async Task<ActionResult> Search()
        {

            HttpClient client = new HttpClient();

            string url = "https://api.deezer.com/chart/0/tracks";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // empty list of track to store tracks
            List<Track> tracksModel = new List<Track> { };

            if ( response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Response success");

                var json = await response.Content.ReadAsStringAsync();


                JObject jsonData = JObject.Parse(json);

                // ger data which is inside the json array
                JArray tracks = (JArray)jsonData["data"];

                // Empty list of track model

                

                foreach (var track in tracks)
                {


                    tracksModel.Add( new Track
                    {
                        Title = track["title"].ToString(),
                        Artist = track["artist"]["name"].ToString(),
                        AlbumArt = track["album"]["cover_big"].ToString()

                    });

                }

            }


            return View(tracksModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}