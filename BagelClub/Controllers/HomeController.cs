using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BagelClub.Models;
using BagelClub.Services;
using Laughlin.Common.Extensions;
using Laughlin.Common.Utilities;

namespace BagelClub.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var bagellers = new BagellerService().FetchAll();

			var model = new HomeModel
			            	{
			            		Bagellers = bagellers,
			            		ShoppingList = new ShoppingListModel
			            		               	{
			            		               		Locations = BagelShopService.BuildFullShoppingList(bagellers)
			            		               	}
			            	};

			return View(model);
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Template()
		{
			return View();
		}

		public ActionResult SendWeekStartReminderEmail()
		{
			var bagellers = new BagellerService().FetchAll();
			new MailController().SendWeekStartReminderEmail(bagellers).DeliverAsync();

			return null;
		}

		public ActionResult SendDayBeforeReminderEmail()
		{
			var bagellerService = new BagellerService();
			var bagellers = bagellerService.FetchAll();
			var nextBageller = bagellers.First();
			//only send the email if the next purchase date is this week
			if (nextBageller.NextPurchaseDate < DateTime.Today.AddDays(7))
			{
				var model = new DayBeforeReminderEmailModel
				            	{
				            		Bageller = nextBageller,
				            		ShoppingList = new ShoppingListModel(BagelShopService.BuildFullShoppingList(bagellers))
				            	};
				new MailController().SendDayBeforeReminderEmail(model).DeliverAsync();

				//Email is sent. Now set the user's next purchase date
				bagellerService.SetNextPurchaseDate(nextBageller);
				bagellerService.Save(nextBageller);
			}

			return null;
		}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BagelsHere(string location)
        {
            var bagellerService = new BagellerService();

            var model = new SendBagelsAreHereEmailModel()
            {
                Location = location,
                Bagellers = bagellerService.FetchAll()
            };
            new MailController().SendBagelsAreHereEmail(model).DeliverAsync();

            return View();
        }
	}
}
