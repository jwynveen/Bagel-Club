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

		public ActionResult SendWeekStartReminderEmail()
		{
			var bagellers = new BagellerService().FetchAll();
			var emailList = string.Join(",", bagellers.Select(x => x.Email));
			//emailList = "jwynveen@laughlin.com";
			var nextBageller = bagellers.First();

			var subject = "This Week's Bageller is: " + nextBageller.Name;
			var body = new StringBuilder();
			body.Append("<h1>Upcoming Bagellers:</h1>");
			body.Append("<table border=\"0\">");
			foreach (var bageller in bagellers.Skip(1))
			{
				body.Append("<tr><th style='text-align:left;'>{0}</th><td>{1}</td></tr>"
									.FormatWith(bageller.NextPurchaseDate.ToShortDateString(), bageller.Name));
			}
			body.Append("</table>");
			EmailUtil.SendHtmlEmail(emailList, null, "Bagel Club <bagelclub@laughlin.com>", subject, body.ToString());
			return null;
		}
	}
}
