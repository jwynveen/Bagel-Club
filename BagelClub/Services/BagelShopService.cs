using System;
using System.Collections.Generic;
using System.Linq;
using BagelClub.Models;
using Laughlin.Common.Extensions;

namespace BagelClub.Services
{
	public static class BagelShopService
	{
		public static IEnumerable<BagelShop> BuildFullShoppingList(IEnumerable<Bageller> bagellers)
		{
			return (from BagelShopType bagelShopType
			        	in Enum.GetValues(typeof (BagelShopType))
			        select BuildIndividualShoppingList(bagelShopType, bagellers)).ToList();
		}

		public static BagelShop BuildIndividualShoppingList(BagelShopType bagelShopType, IEnumerable<Bageller> bagellers)
		{
			var shop = new BagelShop(bagelShopType.GetName());
			foreach (var bageller in bagellers)
			{
				var bagels = string.Join(",",
				                         typeof (Bageller).GetProperty(bagelShopType.ToString()).GetValue(bageller, null).ToString(),
				                         bageller.Brueggers, bageller.Einstein, bageller.Sendiks);
				shop.AddBagel(bagels);
			}
			return shop;
		}
	}
}