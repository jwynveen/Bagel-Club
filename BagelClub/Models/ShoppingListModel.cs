using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BagelClub.Models
{
	public class ShoppingListModel
	{
		public IEnumerable<BagelShop> Locations { get; set; }
	}

	public class BagelShop
	{
		public string Name { get; set; }
		public Dictionary<string, int> Bagels { get; set; }
	}
}