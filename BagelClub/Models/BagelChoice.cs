namespace BagelClub.Models
{
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