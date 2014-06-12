using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eurovision.Models;
using System.Web.Profile;
using System.Web.Security;

namespace Eurovision.Areas.Mobile.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        SourceRepository db = new SQLRepository();
        public GameController()
            : this(new SQLRepository())
        { }
        public GameController(SourceRepository repository)
        {
            db = repository;
        }
        public ActionResult Results()
        {
            IEnumerable<Event> model = db.GetAllEvents().Where(x => x.EuroWinnerID != null || x.HomeChampID != null);
            return View(model);
        }
        public ActionResult Join(int id)
        {
            EventPlayer model = new EventPlayer { Year = id };
            model.PlayerGuid = (Guid)Membership.GetUser().ProviderUserKey;

            int? previousScore = db.GetUserPredictionForYear(id, model.PlayerGuid);
            if (previousScore != null && previousScore > 0)
            {
                model.PredictedUKScore = previousScore;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Join(EventPlayer model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Event = db.GetEventByYear(model.Year);
                    db.AddEventPlayer(model);
                    HttpContext.Profile.SetPropertyValue("CurrentGame", model.Year);
                    var CountryList = db.GetEventCountriesByYear(model.Year);
                    ICollection<PlayerEventCountryScore> PECS = new List<PlayerEventCountryScore>();
                    foreach (var item in CountryList)
                    {
                        PECS.Add(new PlayerEventCountryScore
                        {
                            EventCountryID = item.CountryID,
                            PlayerGuid = model.PlayerGuid,
                            EventCountry = item
                        });
                    }
                    db.StartGameForPlayer(PECS);
                    return RedirectToAction("Play", "Game");
                }
                else
                {
                    throw new Exception("Modelstate invalid");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.InnerException);
                return View(model);
            }
        }
        public ActionResult Play()
        {
            Guid guid = (Guid)Membership.GetUser().ProviderUserKey;
            GameVM model = new GameVM();
            model.Event = db.GetEventByYear((int)HttpContext.Profile.GetPropertyValue("CurrentGame"));
            model.EventPlayer = db.GetCurrentEventPlayer(model.Event.Year, guid);
            model.PlayerScores = db.GetPlayerScoresForYear(model.Event.Year, guid);
            return View(model);
        }
        public ActionResult Score(int id)
        {
            PlayerEventCountryScore model = db.GetPlayerScoreByID(id);
            Guid currentplayer = (Guid)Membership.GetUser().ProviderUserKey;
            if (currentplayer != model.PlayerGuid)
            {
                throw new Exception("You are not the correct player for this record");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Score(PlayerEventCountryScore model)
        {
            //check validity of user
            Guid currentplayer = (Guid)Membership.GetUser().ProviderUserKey;
            if (currentplayer != model.PlayerGuid)
            {
                //invalid user -- throw error 
                throw new Exception("You are not the correct player for this record");
            }
            //check validity of model
            try
            {
                if (ModelState.IsValid)
                {
                    db.RecordPlayerScore(model);
                    return RedirectToAction("Play");
                }
                else
                {
                    throw new Exception("Modelstate.invalid");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex);
                model.EventCountry = db.GetEventCountry(model.EventCountryID);
                return View(model);
            }
        }
    }
}
