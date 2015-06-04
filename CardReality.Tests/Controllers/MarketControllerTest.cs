using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CardReality.Controllers;
using CardReality.Data;
using CardReality.Data.Data;
using CardReality.Data.Models;
using CardReality.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CardReality.Tests.Controllers
{
    [TestClass]
    public class MarketControllerTest
    {
        [TestMethod]
        public void TestIndex_ShouldReturnDatabaseProjection()
        {
            using (var context = ApplicationDbContext.CreateNew())
            {
                Thread.Sleep(60);
                var db = new ApplicationData(context);

                var identity = new GenericIdentity("admin");
                var controller = new MarketController(db);

                var controllerContext = new Mock<ControllerContext>();
                var principal = new Mock<IPrincipal>();
                principal.Setup(p => p.IsInRole("Administrator")).Returns(true);
                principal.SetupGet(x => x.Identity.Name).Returns("admin");

                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
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
                Thread.Sleep(60);
                var db = new ApplicationData(context);

                var identity = new GenericIdentity("admin");
                var user = db.Players.All().FirstOrDefault(p => p.UserName == "admin");
                var controller = new MarketController(db);

                var controllerContext = new Mock<ControllerContext>();
                var principal = new Mock<IPrincipal>();
                principal.Setup(p => p.IsInRole("Administrator")).Returns(true);
                principal.SetupGet(x => x.Identity.Name).Returns("admin");
                
                var request = new Mock<System.Web.HttpRequestBase>();
                var requestParams = new NameValueCollection
                {
                    {"card", user.Deck.Select(d => d.CardId).FirstOrDefault().ToString()},
                    {"price", "1000"}
                };

                request.SetupGet(r => r.Form).Returns(requestParams);

                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
                controller.ControllerContext = controllerContext.Object;
                
                var result = controller.AddOffer();
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

                
            }
        }
    }
}
