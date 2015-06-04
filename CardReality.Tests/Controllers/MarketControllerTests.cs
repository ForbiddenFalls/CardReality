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
using CardReality.Enums;
using CardReality.Services;
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
            LocalizationService.CurrentLanguage = Language.En;
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
        [ExpectedException(typeof(Exception), "Cannot buy your own offer")]
        public void TestBuy_OwnOffer_ShouldThrowException()
        {
            using (var context = ApplicationDbContext.CreateNew())
            {
                var db = new ApplicationData(context);
                var user = db.Players.All().FirstOrDefault(p => p.UserName == this.username);
               
                var offer = new Market()
                {
                    CardName = "TestCardToAdd",
                    Price = 500,
                    Owner = user,
                    SoldOn = DateTime.Now
                };
                db.Offers.Add(offer);
                user.Money = 1200;
                db.SaveChanges();

                var claim = new Claim(this.username, user.Id);
                var mockIdentity =
                    Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
                var principal = new ClaimsPrincipal(mockIdentity);

                var controller = new MarketController(db);
                var controllerContext = new Mock<ControllerContext>();

                var httpContext = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>();
                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                controller.Buy(offer.Id);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid offer")]
        public void TestBuy_NonExistentOffer_ShouldThrowException()
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
                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                controller.Buy(-387);
            }
        }


        [TestMethod]
        public void TestBuy_NonExistentCard_ShouldCreateCard()
        {
            using (var context = ApplicationDbContext.CreateNew())
            {
                var db = new ApplicationData(context);
                var user = db.Players.All().FirstOrDefault(p => p.UserName == this.username);
                var admin = db.Players.All().FirstOrDefault(p => p.UserName == "admin");

                var cardName = "TestCardToAdd" + DateTime.Now.Ticks;

                var offer = new Market()
                {
                    CardName = cardName,
                    Price = 500,
                    Owner = admin,
                    SoldOn = DateTime.Now
                };
                db.Offers.Add(offer);
                user.Money = 600;
                db.SaveChanges();

                var cardExists = db.Cards.All().Any(c => c.Name == cardName);
                Assert.IsFalse(cardExists);

                var claim = new Claim(this.username, user.Id);
                var mockIdentity =
                    Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
                var principal = new ClaimsPrincipal(mockIdentity);

                var controller = new MarketController(db);
                var controllerContext = new Mock<ControllerContext>();

                var httpContext = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>();
                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                var result = controller.Buy(offer.Id);
                Assert.AreEqual(100, user.Money);
                cardExists = db.Cards.All().Any(c => c.Name == cardName);
                Assert.IsTrue(cardExists);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "You do not have enough money")]
        public void TestBuy_NotEnoughMoney_ShouldThrowException()
        {
            using (var context = ApplicationDbContext.CreateNew())
            {
                var db = new ApplicationData(context);
                var user = db.Players.All().FirstOrDefault(p => p.UserName == this.username);
                var admin = db.Players.All().FirstOrDefault(p => p.UserName == "admin");
                var offer = new Market()
                {
                    CardName = "TestCardToAdd",
                    Price = 500,
                    Owner = admin,
                    SoldOn = DateTime.Now
                };
                db.Offers.Add(offer);
                user.Money = 499;
                db.SaveChanges();

                var claim = new Claim(this.username, user.Id);
                var mockIdentity =
                    Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
                var principal = new ClaimsPrincipal(mockIdentity);

                var controller = new MarketController(db);
                var controllerContext = new Mock<ControllerContext>();

                var httpContext = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>();
                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                controller.Buy(offer.Id);
            }
        }

        [TestMethod]
        public void TestBuy_ShouldTakeMoneyCorrectly()
        {
            using (var context = ApplicationDbContext.CreateNew())
            {
                var db = new ApplicationData(context);
                var user = db.Players.All().FirstOrDefault(p => p.UserName == this.username);
                var admin = db.Players.All().FirstOrDefault(p => p.UserName == "admin");
                var offer = new Market()
                {
                    CardName = "TestCardToAdd",
                    Price = 500,
                    Owner = admin,
                    SoldOn = DateTime.Now
                };
                db.Offers.Add(offer);
                user.Money = 800;
                db.SaveChanges();

                var claim = new Claim(this.username, user.Id);
                var mockIdentity =
                    Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
                var principal = new ClaimsPrincipal(mockIdentity);

                var controller = new MarketController(db);
                var controllerContext = new Mock<ControllerContext>();

                var httpContext = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>();
                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                var result = controller.Buy(offer.Id);
                Assert.AreEqual(300, user.Money);
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
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "You do not own this card")]
        public void TestAdd_InvalidCard_ShouldThrowException()
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
                    nv.Add("card", "355");
                    nv.Add("price", "1000");
                    return nv;
                });

                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                controller.AddOffer();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException),
            "Arithmetic operation resulted in an overflow")]
        public void TestAdd_InvalidMoney_ShouldThrowException()
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
                    nv.Add("price", "-386");
                    return nv;
                });

                controllerContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
                controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
                controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);

                controller.ControllerContext = controllerContext.Object;

                controller.AddOffer();
            }
        }

    }
}
