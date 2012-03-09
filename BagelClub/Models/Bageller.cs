using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BagelClub.ViewModels;
using DataAnnotationsExtensions;
using Laughlin.Common.Extensions;
using Newtonsoft.Json;

namespace BagelClub.Models
{
	public class Bageller
	{
		public Bageller()
		{
			Choices = (from BagelShopType bagelShopType in Enum.GetValues(typeof (BagelShopType))
			           select new BagelChoice {Location = new Location {LocationId = (int) bagelShopType}}).ToList();
		}
		public string Id { get; set; }
		[JsonIgnore]
		public int BagellerId
		{
			get { return (Id ?? string.Empty).Replace("bagellers/", string.Empty).ToSafeInt(); }
			set { Id = "bagellers/" + value; }
		}
		[Required]
		public string Name { get; set; }
		[Required]
		[Email]
		public string Email { get; set; }
		[Display(Name = "What you want from where...")]
		public IEnumerable<BagelChoice> Choices { get; set; }

		[Display(Name = "Where are you buying from?")]
		public Location PurchaseLocation { get; set; }

		[Display(Name = "Next Purchase Date")]
		public DateTime NextPurchaseDate { get; set; }

		public IEnumerable<BagelChoice> GetSelection(BagelShopType bagelShopType)
		{
			var orderedChoices = Choices.OrderBy(x => x.Ordering);
			return orderedChoices.Where(x => x.Location.LocationId == (int) bagelShopType)
				.Concat(
					orderedChoices.Where(x => x.Location.LocationId != (int) bagelShopType));
		}

		public override string ToString()
		{
			return Name;
		}
	}
}