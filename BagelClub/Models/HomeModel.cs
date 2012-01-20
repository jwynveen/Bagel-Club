using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BagelClub.Models
{
	public class HomeModel
	{
		public IEnumerable<Bageller> Bagellers { get; set; }
		public ShoppingListModel ShoppingList { get; set; }
	}
}