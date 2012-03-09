using System.Collections.Generic;
using BagelClub.Models;

namespace BagelClub.ViewModels
{
	public class HomeModel
	{
		public IEnumerable<Bageller> Bagellers { get; set; }
		public ShoppingListModel ShoppingList { get; set; }
	}
}