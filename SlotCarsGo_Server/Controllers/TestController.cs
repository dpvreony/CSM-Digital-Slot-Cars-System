using SlotCarsGo_Server.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlotCarsGo_Server.Controllers
{
    public class TestController : Controller
    {
        // GET: TestConnection
        public ActionResult StaticDb()
        {
            try
            {
                ViewBag.Message = TestConnection.ConnectToDBStaticCreds();
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Failed in controller: {e.Message} {e.StackTrace.ToString()}";
            }
            return View();
        }

        // GET: TestConnection
        public ActionResult WebConfigDb()
        {
            try
            {
                ViewBag.Message = TestConnection.ConnectToDBFromWebConfig();
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Failed in controller: {e.Message} {e.StackTrace.ToString()}";
            }
            return View();
        }

        // GET: TestConnection
        public ActionResult EntityDb()
        {
            try
            {
                ViewBag.Message = TestConnection.EntityDB();
            }
            catch (Exception e)
            {
                ViewBag.Message = $"Failed in controller: {e.Message} {e.StackTrace.ToString()}";
            }
            return View();
        }
        
    }
}