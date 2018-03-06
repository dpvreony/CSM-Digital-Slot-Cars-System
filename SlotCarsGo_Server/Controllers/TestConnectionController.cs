using SlotCarsGo_Server.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlotCarsGo_Server.Controllers
{
    public class TestConnectionController : Controller
    {
        // GET: TestConnection
        public ActionResult Index()
        {
            try
            {
                ViewBag.Message = TestConnection.ConnectToDB();
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Failed in controller: {e.Message} {e.StackTrace.ToString()}";
            }
            return View();
        }
    }
}