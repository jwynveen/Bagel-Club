using System;

namespace BagelClub.Models
{
	public class Bageller
	{
		public int BagellerId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Brueggers { get; set; }
		public string Einstein { get; set; }
		public string Sendiks { get; set; }
		public Byte PurchaseLocation { get; set; }
		public DateTime NextPurchaseDate { get; set; }

		public string GetSelection(BagelShopType bagelShopType)
		{
			return string.Join(",",
			                   typeof (Bageller).GetProperty(bagelShopType.ToString()).GetValue(this, null).ToString(),
			                   Brueggers, Einstein, Sendiks);
		}
		public override string ToString()
		{
			return Name;
		}
	}
}