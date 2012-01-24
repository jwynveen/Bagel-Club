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
	}
}