using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BagelClub.Models;
using BagelClub.Services;

namespace BagelClub.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var model = new HomeModel
							{
								Bagellers = new BagellerService().FetchAll(),
								ShoppingList = new ShoppingListModel
								               	{
								               		Locations = new List<BagelShop>()
								               	}
							};

			return View(model);
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
