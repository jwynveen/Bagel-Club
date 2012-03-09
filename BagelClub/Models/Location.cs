using BagelClub.ViewModels;
using Laughlin.Common.Extensions;
using Newtonsoft.Json;

namespace BagelClub.Models
{
	public class Location
	{
		public string Id { get; set; }
		[JsonIgnore]
		public int LocationId
		{
			get { return (Id??string.Empty).Replace("locations/", string.Empty).ToSafeInt(); }
			set { Id = "locations/" + value; }
		}
		public string Name { get; set; }
		public BagelShopType BagelShop { get { return (BagelShopType) LocationId; } }
	}
}