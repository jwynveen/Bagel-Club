using System.Collections.Generic;
using System.Web.Mvc;
using BagelClub.Models;

namespace BagelClub.ViewModels
{
	public class BagellerEditModel
	{
		public Bageller Item { get; set; }
		public IEnumerable<SelectListItem> Locations { get; set; }
	}
}