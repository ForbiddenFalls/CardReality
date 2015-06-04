using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using CardReality.Controllers;
using CardReality.Data;
using CardReality.Data.Data;
using CardReality.Data.Models;
using CardReality.Models;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CardReality.Tests.Controllers
{
    [TestClass]
    public class MarketControllerTests
    {
        private string username;

        public MarketControllerTests()
        {
            string username = "test_user_" + DateTime.Now.Ticks;
            using (var context = ApplicationDbContext.CreateNew())
            {
                context.CreateUser("user", username, username + "@abv.bg", "#Lo6omie");
                context.SaveChanges();
                this.username = username;
            }
        }

        [TestMethod]
        public void TestIndex_ShouldReturnDatabaseProjection()
        {
            using (var context = ApplicationDbContext.CreateNew())
            {
                var db = new ApplicationData(context);
                var controller = new MarketController(db);

                var controllerContext = new Mock<ControllerContext>();
                controller.ControllerContext = controllerContext.Object;

                var result = controller.Index();
                Assert.IsInstanceOfType(result, typeof(ViewResult));

                var data = (result as ViewResult);
                var viewResult = (IQueryable<Market>)data.ViewData.Model;
                Assert.AreEqual(viewResult.Count(), db.Offers.All().Count());
            }
        }

        [TestMethod]
        public void TestAdd_ShouldHaveMoreThanZeroOffers()
        {
            using (var context = ApplicationDbContext.CreateNew())
            {
                var db = new ApplicationData(context);
                var user = db.Players.All().FirstOrDefault(p => p.UserName == this.username);

                var claim = new Claim(this.username, user.Id);
                var mockIdentity =
                    Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
                var principal = new ClaimsPrincipal(mockIdentity);
                
                var controller = new MarketController(db);
                var controllerContext = new Mock<ControllerContext>();
               
                var httpContext = new Mock<HttpContextBase>();

                var request = new Mock<HttpRequestBase>();
                request.SetupGet(x => x.Form).Returns(delegate()
                {
                    var nv = new NameValueCollection();
                    nv.Add("card", user.Deck.Select(d => d.CardId).FirstOrDefault().ToString());
                    nv.Add("price", "1000");
                    return nv;
                });
                
                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                var result = controller.AddOffer();
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));            }
        }


    }
}
