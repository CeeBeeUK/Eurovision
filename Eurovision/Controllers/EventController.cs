using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eurovision.Models;

namespace Eurovision.Controllers
{
    public class EventController : Controller
    {
        SourceRepository db = new SQLRepository();
        public EventController()
            : this(new SQLRepository())
        { }
        public EventController(SourceRepository repository)
        {
            db = repository;
        }

        public ActionResult Details(int id)
        {
            IEnumerable<EventWithScoresVM> model = db.GetScoresForYear(id);
            return View(model);
        }

    }
}
