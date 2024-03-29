﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RhythmRecall
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "testApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "testAPI2",
            routeTemplate: "api/{controller}/{action}/{userId}/{trackId}",
            defaults: new { userId = RouteParameter.Optional, trackId = RouteParameter.Optional }
            );



        }
    }
}
