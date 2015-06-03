using System.Linq;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using CardReality.Data.Data;
using CardReality.Data.Models;

namespace CardReality.Controllers
{
    public class CardsController : BaseController
    {
        public CardsController(IApplicationData data)
            :base(data)
        {
        }

        // GET: Cards
        [Authorize]
        public ActionResult Index()
        {
            Player player = this.Data.Players.Find(User.Identity.GetUserId());
            var playerCards = player.Deck.Select(c => c.Card);
            return View(playerCards);
        }
    }
}