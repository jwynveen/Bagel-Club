using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Laughlin.Common.Extensions;

namespace BagelClub.Models
{
	public class ShoppingListModel
	{
		public ShoppingListModel(){}
		public ShoppingListModel(IEnumerable<BagelShop> locations) : this(locations, null){}
		public ShoppingListModel(IEnumerable<Bageller> bagellers) : this(null, bagellers){}
		public ShoppingListModel(IEnumerable<BagelShop> locations, IEnumerable<Bageller> bagellers)
		{
			Locations = locations;
			Bagellers = bagellers.OrderBy(x => x.Name);
		}
		public IEnumerable<BagelShop> Locations { get; set; }
		public IEnumerable<Bageller> Bagellers { get; set; }
	}

	public class BagelShop
	{
		public BagelShop(BagelShopType type)
		{
			Type = type;
			Bagels = new List<Bagel>();
		}
		public string Name { get { return Type.GetName(); } }
		public BagelShopType Type { get; set; }
		public List<Bagel> Bagels { get; set; }
		public void AddBagel (string bagel)
		{
			var firstChoice = bagel;
			var secondChoice = string.Empty;
			var bagels = bagel.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
			if (bagels.Length > 1)
			{
				firstChoice = bagels[0];
				for (int i = 1; i < bagels.Length; i++)
				{
					if (bagels[i].Trim().SafeEquals(firstChoice, StringComparison.OrdinalIgnoreCase)) continue;
					secondChoice = bagels[i].Trim();
					break;
				}
			}


			if (Bagels.Any(x => x.Name.SafeEquals(firstChoice)))
			{
				var item = Bagels[Bagels.FindIndex(x => x.Name.SafeEquals(firstChoice))];
				item.Quantity++;
				if (!secondChoice.IsNullOrEmpty())
				{
					if (item.Alternates == null)
						item.Alternates = new List<string> {secondChoice};
					else
						item.Alternates.Add(secondChoice);
				}
			}
			else
			{
				Bagels.Add(secondChoice.IsNullOrEmpty()
				           	? new Bagel(firstChoice)
				           	: new Bagel(firstChoice, 1, new List<string>{secondChoice}));
			}
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

	public class Bagel
	{
		public Bagel(){}
		public Bagel(string name) : this(name, 1){}
		public Bagel(string name, int quantity) : this(name, quantity, null){}
		public Bagel(string name, int quantity, List<string> alternates)
		{
			Name = name;
			Quantity = quantity;
			Alternates = alternates;
		}
		public string Name { get; set; }
		public int Quantity { get; set; }
		public List<string> Alternates { get; set; }
	}
}