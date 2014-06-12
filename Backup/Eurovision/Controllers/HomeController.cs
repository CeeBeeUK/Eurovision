using System;
using System.Web.Mvc;
using System.Web.Security;
using Eurovision.Models;

namespace Eurovision.Controllers
{
    public class HomeController : Controller
    {
        SourceRepository db = new SQLRepository();
        public HomeController()
            : this(new SQLRepository())
        { }
        public HomeController(SourceRepository repository)
        {
            db = repository;
        }
 
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int CurrentGame=(int)HttpContext.Profile.GetPropertyValue("CurrentGame");
                if (CurrentGame > 0)
                {
                    Guid player = (Guid)Membership.GetUser().ProviderUserKey;
                    EventPlayer ep = db.GetCurrentEventPlayer(CurrentGame, player);
                    if (ep != null)
                    {
                        //user is authenticated and in a game... go there!
                        return RedirectToAction("Play", "Game");
                    }
                    HttpContext.Profile.SetPropertyValue("CurrentGame",0);
                    //User is authenticated but hasn't joined a game, so...
                    return RedirectToAction("Join", "Game", new { id = DateTime.Now.Year });
                }
                else
                {
                    //User is authenticated but hasn't joined a game, so...
                    return RedirectToAction("Join", "Game", new { id = DateTime.Now.Year });
                }
            }
            //not authenticated... show home page
            return View();
        }
    }
}
