using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BagelClub.Models;
using BagelClub.Services;
using BagelClub.ViewModels;
using Laughlin.Common.Extensions;
using Laughlin.Common.Utilities;
using StructureMap;

namespace BagelClub.Controllers
{
	public class HomeController : Controller
	{
		private readonly IBagellerService _bagellerService;
		public HomeController(IBagellerService bagellerService)
		{
			_bagellerService = bagellerService;
		}

		public ActionResult Index()
		{
			var bagellers = _bagellerService.FetchAll();

			var model = new HomeModel
			            	{
			            		Bagellers = bagellers,
			            		ShoppingList = new ShoppingListModel(BagelShopService.BuildFullShoppingList(bagellers), bagellers)
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
			var bagellerService = new BagellerService();
			var bagellers = bagellerService.FetchAll();
			var nextBageller = bagellers.First();
			//set the next purchase dates for bagellers who have already made their purchase
			while (nextBageller.NextPurchaseDate.IsBefore(DateTime.Now))
			{
				bagellerService.SetNextPurchaseDate(nextBageller);
				bagellerService.Save(nextBageller);

				bagellers = bagellerService.FetchAll().OrderBy(x => x.NextPurchaseDate);
				nextBageller = bagellers.First();
			}
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
			}

			return null;
		}

		[HttpPost]
		public ActionResult BagelsHere(string location)
		{
			var bagellerService = new BagellerService();

			var model = new SendBagelsAreHereEmailModel()
			{
				Location = location,
				Bagellers = bagellerService.FetchAll()
			};
			new MailController().SendBagelsAreHereEmail(model).DeliverAsync();

			return View(model);
		}
	}
}
