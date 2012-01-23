using FluentScheduler;
using Laughlin.Common.Utilities;

namespace BagelClub.Tasks
{
	public class KeepAliveTask : ITask
	{
		public void Execute()
		{
			//System.Diagnostics.Debugger.Launch();
			EmailUtil.SendTextEmail("jwynveen@laughlin.com", null, "Bagel Club <bagelclub@laughlin.com>", "Keep Alive", "Keeping app pool alive...");
		}
	}
}