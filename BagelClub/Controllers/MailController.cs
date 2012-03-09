using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ActionMailer.Net.Mvc;
using BagelClub.Models;
using BagelClub.ViewModels;
using Laughlin.Common.Extensions;

namespace BagelClub.Controllers
{
	public class MailController : MailerBase
	{
		private const string BagelClubFromEmail = "Bagel Club <bagelclub@laughlin.com>";

		public EmailResult SendWeekStartReminderEmail(IEnumerable<Bageller> model)
		{
			From = BagelClubFromEmail;
			var emailList = string.Join(",", model.Select(x => x.Email));
			if (!Environment.MachineName.SafeEquals(ConfigurationManager.AppSettings["RunScheduledTasksFromMachineName"], StringComparison.OrdinalIgnoreCase))
				emailList = "jwynveen@laughlin.com,tjansen@laughlin.com";
			
			//They could have multiple emails in the Email column, so join them and then split them
			foreach (var email in emailList.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
				To.Add(email);

			var nextBageller = model.First();
			Subject = "This Week's Bageller is: " + nextBageller.Name;

			return Email("SendWeekStartReminderEmail", model);
		}

		public EmailResult SendDayBeforeReminderEmail(DayBeforeReminderEmailModel model)
		{
			From = BagelClubFromEmail;
			To.Add(!Environment.MachineName.SafeEquals(ConfigurationManager.AppSettings["RunScheduledTasksFromMachineName"], StringComparison.OrdinalIgnoreCase)
					? "jwynveen@laughlin.com,tjansen@laughlin.com"
					: model.Bageller.Email);
			BCC.Add("jwynveen@laughlin.com,tjansen@laughlin.com");
			Subject = "Don't forget the bagels!";

			return Email("SendDayBeforeReminderEmail", model);
		}

		public EmailResult SendBagelsAreHereEmail(SendBagelsAreHereEmailModel model)
		{
			string emailList = string.Empty;
			emailList = !Environment.MachineName.SafeEquals(ConfigurationManager.AppSettings["RunScheduledTasksFromMachineName"], StringComparison.OrdinalIgnoreCase)
					? "jwynveen@laughlin.com,tjansen@laughlin.com"
					: string.Join(",", model.Bagellers.Select(x => x.Email));

			//They could have multiple emails in the Email column, so join them and then split them
			foreach (var email in emailList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				To.Add(email);

			From = BagelClubFromEmail;
			Subject = "Bagels are here!";
			return Email("SendBagelsAreHereEmail", model);
		}

		public EmailResult WelcomeEmail(WelcomeEmailModel model)
		{
			string emailList = string.Empty;
			emailList = !Environment.MachineName.SafeEquals(ConfigurationManager.AppSettings["RunScheduledTasksFromMachineName"], StringComparison.OrdinalIgnoreCase)
					? "jwynveen@laughlin.com,tjansen@laughlin.com"
					: string.Join(",", model.Bageller.Email);
			BCC.Add("jwynveen@laughlin.com,tjansen@laughlin.com");

			//They could have multiple emails in the Email column, so join them and then split them
			foreach (var email in emailList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				To.Add(email);

			From = BagelClubFromEmail;
			Subject = "Welcome to Bagel Club!";
			return Email("WelcomeEmail", model);
		}

	}
}
