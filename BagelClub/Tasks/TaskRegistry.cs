using System;
using FluentScheduler;

namespace BagelClub.Tasks
{
	public class TaskRegistry : Registry
	{
		public TaskRegistry()
		{
			Schedule<WeekStartReminderTask>().ToRunEvery(1).Weeks().On(DayOfWeek.Monday).At(8, 45);	//Mondays @ 8:00am
			//Schedule<WeekStartReminderTask>().ToRunEvery(5).Minutes();
		}
	}
}