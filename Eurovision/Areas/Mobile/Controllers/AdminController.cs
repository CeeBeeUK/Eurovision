using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eurovision.Models;
using System.Web.Security;

namespace Eurovision.Areas.Mobile.Controllers
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
                model.Countries = new SelectList(db.GetAllCountries(), "id", "Name");
                return View(model);
            }
        }
        public ActionResult AssignPlayer(int id)
        {
            EventCountry EC = db.GetEventCountry(id);

            AssignPlayerVM model = new AssignPlayerVM
            {
                EventCountry = EC,
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
                db.AllocateCountryToPlayer(model.EventCountry.id, model.PlayerGuid);
                return RedirectToAction("EventDetails", new { id = EC.Event.Year });
            }
            catch (Exception ex)
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
        public ActionResult SetWinners(int id)
        {
            EventWinnerVM model = new EventWinnerVM();
            model.Event = db.GetEventByYear(id);
            model.EventCountries = new SelectList(db.GetEventCountriesByYear(id), "id", "Description");
            return View(model);
        }
        [HttpPost]
        public ActionResult SetWinners(EventWinnerVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Exception result = db.SetWinners(model);
                    if (result == null)
                    {
                        return RedirectToAction("Results", "Game", new { area = "" });
                    }
                    else
                    {
                        throw result;
                    }
                }
                //get model invalid errors
                string messages = string.Join(" ", ModelState.Values
                                                        .SelectMany(x => x.Errors)
                                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError("Error", string.Format("The following validation errors occurred:{0}", messages));
                model.Event = db.GetEventByYear(model.Event.Year);
                model.EventCountries = new SelectList(db.GetEventCountriesByYear(model.Event.Year), "id", "Description");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.InnerException.Message);
                model.Event = db.GetEventByYear(model.Event.Year);
                model.EventCountries = new SelectList(db.GetEventCountriesByYear(model.Event.Year), "id", "Description");
                return View(model);
            }
        }

    }
}

