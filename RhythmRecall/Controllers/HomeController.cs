using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using RhythmRecall.Models;

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