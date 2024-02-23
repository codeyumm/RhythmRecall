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

        // GET - Home/Search/{id}
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


        // GET - localhost:44387/home/tracksearch
        // to handle user search for a track
        public async Task<ActionResult> TrackSearch(string searchQuery)
        {

            // get the search string from request
            string searchString = searchQuery;

            // client object to make request
            HttpClient client = new HttpClient();

            // api url
            string url = $"https://api.deezer.com/search?q={searchString}";
            Debug.WriteLine(url);

            // empty list of track
            List<Track> trackList = new List<Track> { };

            // get response after making the request with api url
            HttpResponseMessage response = client.GetAsync(url).Result;

            // check if response was succesful or not
            if (response.IsSuccessStatusCode)
            {

                // store data as string
                var json = await response.Content.ReadAsStringAsync();

                // convert string into json
                JObject jsonData = JObject.Parse(json);

                // get data which is inside the json array
                JArray tracks = (JArray)jsonData["data"];

                foreach (var track in tracks)
                {


                    trackList.Add(new Track
                    {
                        Title = track["title"].ToString(),
                        Artist = track["artist"]["name"].ToString(),
                        AlbumArt = track["album"]["cover_big"].ToString()

                    });


                }

             
            }

            return View(trackList);
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