using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eurovision.Models;

namespace Eurovision.Controllers
{
    public class CheckMyController : Controller
    {
        SourceRepository db = new SQLRepository();
        public CheckMyController()
            : this(new SQLRepository())
        { }
        public CheckMyController(SourceRepository repository)
        {
            db = repository;
        }

        public ActionResult Wackiest(int id)
        {
            PlayerEventCountryScore pecs = db.GetPlayerScoreByID(id);

            //get current user
            Guid playerID = pecs.PlayerGuid;

            //get any fattest scores==true 
            var Wackos = db.GetPlayerScoresForYear(pecs.EventCountry.Event.Year, playerID).Where(x => x.Wackiest == true);
            if (Wackos.Count() > 0)
            {
                return Json(new { success = true, matches = Wackos.Select(x => x.EventCountry.Country.Name) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Fattest(int id)
        {
            PlayerEventCountryScore pecs = db.GetPlayerScoreByID(id);

            //get current user
            Guid playerID = pecs.PlayerGuid;

            //get any fattest scores==true 
            var FatScores = db.GetPlayerScoresForYear(pecs.EventCountry.Event.Year, playerID).Where(x => x.Fattest == true);
            if (FatScores.Count() > 0)
            {
                return Json(new { success = true, matches = FatScores.Select(x => x.EventCountry.Country.Name) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Wail(int id)
        {
            PlayerEventCountryScore pecs = db.GetPlayerScoreByID(id);

            //get current user
            Guid playerID = pecs.PlayerGuid;

            //get any fattest scores==true 
            var Wailers = db.GetPlayerScoresForYear(pecs.EventCountry.Event.Year, playerID).Where(x => x.BestWail == true);
            if (Wailers.Count() > 0)
            {
                return Json(new { success = true, matches = Wailers.Select(x => x.EventCountry.Country.Name) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
