using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JiraIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var getResult = ITFCJIRAPI.JiraMethods.createissue("Test - Newsummary", "Test - description", "The President", "Request",
               new List<string> { "152256", "230219", "e10325", "310055", "901290", "901506" });

            if (getResult.GetType() == typeof(ITFCJIRAPI.issueresponse))
            {
                var create_response = (ITFCJIRAPI.issueresponse)getResult;
            }
            var getissue = ITFCJIRAPI.JiraMethods.readissue("ITFC-57");
            if (getissue.GetType() == typeof(ITFCJIRAPI.getissuedetails))
            {   
                var create_response = (ITFCJIRAPI.getissuedetails)getissue;
            }
           // var updateissue = ITFCJIRAPI.JiraMethods.updateissue("ITFC-62", "summary123_2222", "descriptiondddd", "The President", "Request", new List<string> { "152256", "e10325" });

         // var getStatus = ITFCJIRAPI.JiraMethods.AddAttachments("ITFC-62", new List<string>() { @"C:\Users\shivakumar\Desktop\ITFCJira\TextDocument.txt" });

            return View();
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