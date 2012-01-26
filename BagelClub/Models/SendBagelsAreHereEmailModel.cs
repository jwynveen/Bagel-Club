using System.Collections.Generic;

namespace BagelClub.Models
{
	public class SendBagelsAreHereEmailModel
	{
        public string Location { get; set; }
        public IEnumerable<Bageller> Bagellers { get; set; }
	}
}