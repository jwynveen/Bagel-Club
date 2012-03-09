using BagelClub.Models;

namespace BagelClub.ViewModels
{
	public class DayBeforeReminderEmailModel
	{
		public Bageller Bageller { get; set; }
		public ShoppingListModel ShoppingList { get; set; }
	}
}