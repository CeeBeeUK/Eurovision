using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eurovision.Models;
using System.Web.Profile;

namespace Eurovision.Controllers
{
    
    public class MyController : Controller
    {
        public ActionResult Profile()
        {
            ProfileBase currentProfile = HttpContext.Profile;
            ProfileVM model = new ProfileVM();
            model.DisplayName = currentProfile.GetPropertyValue("DisplayName").ToString();
            model.CurrentGame = (int)currentProfile.GetPropertyValue("CurrentGame");
            return View(model);
        }
        [HttpPost]
        public ActionResult Profile(ProfileVM model)
        {
            ProfileBase current =  HttpContext.Profile;
            if (current.GetPropertyValue("DisplayName") != model.DisplayName)
            {
                current.SetPropertyValue("DisplayName", model.DisplayName);
            }
            if ((int)current.GetPropertyValue("CurrentGame") != model.CurrentGame)
            {
                current.SetPropertyValue("CurrentGame", model.CurrentGame);
            }
            return View(model);
        }
        public ActionResult SwapShowNotes()
        {
            bool curVal = (bool)HttpContext.Profile.GetPropertyValue("ShowNotes");
            HttpContext.Profile.SetPropertyValue("ShowNotes", !curVal);
            return Json(new { newVal = !curVal }, JsonRequestBehavior.AllowGet);
        }
    }
}
