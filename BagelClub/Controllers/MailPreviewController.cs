using System;
using System.Web.Mvc;
using BagelClub.Services;
using Laughlin.Common.Extensions;

namespace BagelClub.Controllers
{
	public class MailPreviewController : Controller
	{
		public ActionResult WeekStartReminderEmail(string id)
		{
			var bagellers = new BagellerService().FetchAll();
			if (id.SafeEquals("text", StringComparison.OrdinalIgnoreCase))
				return View("~/Views/Mail/SendWeekStartReminderEmail.txt.cshtml", bagellers);
			return View("~/Views/Mail/SendWeekStartReminderEmail.html.cshtml", bagellers);
		}

	}
}
