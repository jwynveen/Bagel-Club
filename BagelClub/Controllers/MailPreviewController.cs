using System;
using System.Linq;
using System.Web.Mvc;
using BagelClub.Models;
using BagelClub.Services;
using BagelClub.ViewModels;
using Laughlin.Common.Extensions;

namespace BagelClub.Controllers
{
	public class MailPreviewController : Controller
	{
		private readonly IBagellerService _bagellerService;

		public MailPreviewController(IBagellerService bagellerService)
		{
			_bagellerService = bagellerService;
		}

		public ActionResult WeekStartReminderEmail(string id)
		{
			var bagellers = new BagellerService().FetchAll();
			if (id.SafeEquals("text", StringComparison.OrdinalIgnoreCase))
				return View("~/Views/Mail/SendWeekStartReminderEmail.txt.cshtml", bagellers);
			return View("~/Views/Mail/SendWeekStartReminderEmail.html.cshtml", bagellers);
		}

		public ActionResult DayBeforeReminderEmail(string id)
		{
			var bagellers = new BagellerService().FetchAll();
			var model = new DayBeforeReminderEmailModel
							{
								Bageller = bagellers.First(),
								ShoppingList = new ShoppingListModel(BagelShopService.BuildFullShoppingList(bagellers))
							};
			if (id.SafeEquals("text", StringComparison.OrdinalIgnoreCase))
				return View("~/Views/Mail/SendDayBeforeReminderEmail.txt.cshtml", model);
			return View("~/Views/Mail/SendDayBeforeReminderEmail.html.cshtml", model);
		}

		public ActionResult SendBagelsAreHereEmail(string id)
		{
			var model = new SendBagelsAreHereEmailModel()
			{
				Location = "The .NET Development Area"
			};
			if (id.SafeEquals("text", StringComparison.OrdinalIgnoreCase))
				return View("~/Views/Mail/SendBagelsAreHereEmail.txt.cshtml", model);
			return View("~/Views/Mail/SendBagelsAreHereEmail.html.cshtml", model);
		}

		public ActionResult WelcomeEmail(string id)
		{
			var item = _bagellerService.FetchByBagellerId(1);
			var model = new WelcomeEmailModel {Bageller = item};
			if (id.SafeEquals("text", StringComparison.OrdinalIgnoreCase))
				return View("~/Views/Mail/WelcomeEmail.txt.cshtml", model);
			return View("~/Views/Mail/WelcomeEmail.html.cshtml", model);
		}

	}
}
