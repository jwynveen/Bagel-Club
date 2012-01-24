using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BagelClub.Models
{
	public class ShoppingListModel
	{
		public ShoppingListModel(){}
		public ShoppingListModel(IEnumerable<BagelShop> locations)
		{
			Locations = locations;
		}
		public IEnumerable<BagelShop> Locations { get; set; }
	}

	public class BagelShop
	{
		public BagelShop(string name)
		{
			Name = name;
			Bagels = new Dictionary<string, int>();
		}
		public string Name { get; set; }
		public Dictionary<string, int> Bagels { get; set; }
		public void AddBagel (string bagel)
		{
			var firstChoice = bagel;
			var bagels = bagel.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
			if (bagels.Length > 1)
				firstChoice = bagels[0];

			if (Bagels.ContainsKey(firstChoice))
			{
				Bagels[firstChoice]++;
			}
			else
				Bagels.Add(firstChoice, 1);
		}
	}

	public enum BagelShopType
	{
		[Display(Name = "Bruegger's")]
		Brueggers,
		[Display(Name = "Einstein")]
		Einstein,
		[Display(Name = "Sendik's")]
		Sendiks
	}
}