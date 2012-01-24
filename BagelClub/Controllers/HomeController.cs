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

		public ActionResult SendWeekStartReminderEmail()
		{
			var bagellers = new BagellerService().FetchAll();
			new MailController().SendWeekStartReminderEmail(bagellers).DeliverAsync();

			return null;
		}
	}
}
