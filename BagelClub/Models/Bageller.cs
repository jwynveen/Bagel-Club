using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Laughlin.Common.Extensions;

namespace BagelClub.Models
{
	public class Bageller
	{
		public string Id { get; set; }
		public int BagellerId { get { return Id.Replace("bagellers/", string.Empty).ToSafeInt(); } }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Email { get; set; }
		public IEnumerable<BagelChoice> Choices { get; set; }
//		public string Brueggers { get; set; }
//		public string Einstein { get; set; }
//		public string Sendiks { get; set; }
		[Display(Name = "Purchase Location")]
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

	public class Location
	{
		public string Id { get; set; }
		public int LocationId
		{
			get { return (Id??string.Empty).Replace("locations/", string.Empty).ToSafeInt(); }
			set { Id = "locations/" + value; }
		}
		public string Name { get; set; }
		public BagelShopType BagelShop { get { return (BagelShopType) LocationId; } }
	}

	public class BagelChoice
	{
		public string Bagel { get; set; }
		public Location Location { get; set; }
		public int Ordering { get; set; }
		public override string ToString()
		{
			return Bagel;
		}
	}
}