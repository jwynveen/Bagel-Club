﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActionMailer.Net.Mvc;
using BagelClub.Models;

namespace BagelClub.Controllers
{
	public class MailController : MailerBase
	{
		public EmailResult SendWeekStartReminderEmail(IEnumerable<Bageller> model)
		{
			From = "Bagel Club <bagelclub@laughlin.com>";
			var emailList = string.Join(",", model.Select(x => x.Email));
			if (HttpContext.Current.Request.IsLocal)
				emailList = "jwynveen@laughlin.com";
			
			//They could have multiple emails in the Email column, so join them and then split them
			foreach (var email in emailList.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
				To.Add(email);

			var nextBageller = model.First();
			Subject = "This Week's Bageller is: " + nextBageller.Name;

			return Email("SendWeekStartReminderEmail", model);
		}

	}
}
