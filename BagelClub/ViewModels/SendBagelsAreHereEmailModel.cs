using System.Collections.Generic;
using BagelClub.Models;

namespace BagelClub.ViewModels
{
	public class SendBagelsAreHereEmailModel
	{
        public string Location { get; set; }
        public IEnumerable<Bageller> Bagellers { get; set; }
	}
}