using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eurovision.Models;
using System.Web.Security;
using System.Web.UI;

namespace Eurovision.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        SourceRepository db = new SQLRepository();
        public AdminController()
            : this(new SQLRepository())
        { }
        public AdminController(SourceRepository repository)
        {
            db = repository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListCountries()
        {
            return View();
        }
        public ActionResult ListEvents()
        {
            var model = db.GetAllEvents();
            return View(model);
        }
        public ActionResult CreateEvent()
        {
            CreateEventVM model = new CreateEventVM();
            model.Countries = new SelectList(db.GetAllCountries(), "id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateEvent(CreateEventVM model)
        {
            if (ModelState.IsValid)
            {
                db.CreateEvent(model.Event);
                return RedirectToAction("ListEvents");
            }

            return View(model);
        }
        public ActionResult EventDetails(int id)
        {
            EventVM model = new EventVM();
            model.Event = db.GetEventByYear(id);
            model.EventCountries = db.GetEventCountriesByYear(id);
            return View(model);
        }
        public ActionResult AddCountry(int id)
        {
            AddCountryVM model = new AddCountryVM { Year = id };
            model.Countries = new SelectList(db.GetAllCountries(), "id", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult AddCountry(AddCountryVM model)
        {
            try
            {
                db.AddCountryToEvent(model);
                return RedirectToAction("EventDetails", new { id = model.Year });
            }
            catch
            {
                model.Countries = new SelectList(db.GetAllCountries(),"id", "Name");
                return View(model);
            }
        }
        public ActionResult AssignPlayer(int id)
        {
            EventCountry EC = db.GetEventCountry(id);

            AssignPlayerVM model = new AssignPlayerVM 
            {
             EventCountry=EC,
             Players = new SelectList(db.GetPlayersForYear(EC.Event.Year), "PlayerGuid", "Name")
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AssignPlayer(AssignPlayerVM model)
        {
            EventCountry EC = db.GetEventCountry(model.EventCountry.id);
            try
            {
                db.AllocateCountryToPlayer(model.EventCountry.id,model.PlayerGuid);
                return RedirectToAction("EventDetails", new { id = EC.Event.Year });
            }
            catch(Exception ex)
            {
                model.EventCountry = EC;
                model.Players = new SelectList(db.GetPlayersForYear(model.EventCountry.Event.Year), "id", "Name");
                return View(model);
            }
        }
        public ActionResult Users()
        {
            IEnumerable<MembershipUser> model = Membership.GetAllUsers().Cast<MembershipUser>().AsEnumerable();
            return View(model);
        }
        public ActionResult ResetPassword(Guid id)
        {
            var user = Membership.GetUser(id);
            string newPWD = "";
            try
            {
                newPWD = user.ResetPassword();
            }
            catch
            {
                newPWD = "Reset failed";
            }
            return Json(new { result = newPWD }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, OutputCache(Location = OutputCacheLocation.Server, Duration = 0)]
        public ActionResult Unlock(Guid id)
        {
            var user = Membership.GetUser(id);
            user.UnlockUser();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
