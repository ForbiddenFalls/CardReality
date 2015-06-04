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

namespace CardReality.Tests.Controllers
{
    [TestClass]
    public class MarketControllerTests
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
                var user = db.Players.All().FirstOrDefault(p => p.UserName == "admin");

                var claim = new Claim("admin", user.Id);
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
