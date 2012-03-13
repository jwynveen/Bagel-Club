using System;
using System.Collections.Generic;
using System.Linq;
using BagelClub.Models;
using Laughlin.Common.Extensions;
using Omu.ValueInjecter;
using Raven.Client.Document;

namespace BagelClub.Services
{
	public interface IBagellerService
	{
		IEnumerable<Bageller> FetchAll();
		Bageller FetchById(string id);
		Bageller FetchByBagellerId(int id);
		Bageller GetLastBageller();
		void SetNextPurchaseDate(Bageller nextBageller);
		DateTime ResetNextPurchaseDates(int addedWeeks = 0);

		Bageller Save(Bageller item);
		Bageller Delete(int id);
	}
	public class BagellerService : IBagellerService
	{
		private readonly DocumentStore _documentStore;
		public BagellerService()
		{
			_documentStore = new DocumentStore { ConnectionStringName  = "RavenDB"};
			_documentStore.Initialize();
		}

		public Bageller FetchById(string id)
		{
			Bageller item;
			using (var session = _documentStore.OpenSession())
			{
				item = session.Load<Bageller>(id);
			}
			return item;
		}
		public Bageller FetchByBagellerId(int id)
		{
			return FetchById("bagellers/" + id);
		}

		public IEnumerable<Bageller> FetchAll()
		{
			IEnumerable<Bageller> items;
			using (var session = _documentStore.OpenSession())
			{
				items = from bageller in session.Query<Bageller>()
				        orderby bageller.NextPurchaseDate ascending
				        select bageller;
			}
			return items;
		}

		public Bageller GetLastBageller()
		{
			Bageller lastBageller;
			using (var session = _documentStore.OpenSession())
			{
				lastBageller = (from bageller in session.Query<Bageller>()
				                orderby bageller.NextPurchaseDate descending
				                select bageller).FirstOrDefault();
			}
			return lastBageller;
		}

		public void SetNextPurchaseDate(Bageller nextBageller)
		{
			var lastBageller = GetLastBageller();
			nextBageller.NextPurchaseDate = lastBageller.NextPurchaseDate.AddDays(7);
			return;
		}

		public Bageller Save(Bageller item)
		{
			var resetDates = false;
			using (var session = _documentStore.OpenSession())
			{
				if (item.BagellerId == 0)
				{
					//set the Id field
					var queryable = (from bageller in session.Query<Bageller>()
					                orderby bageller.Id descending
					                select bageller.Id).ToList();
					var orderedQueryable = queryable.Select(x => x.Replace("bagellers/", "").ToSafeInt()).OrderByDescending(x => x);
					var lastId = orderedQueryable.Take(1).First();
					item.BagellerId = lastId + 1;

					item.NextPurchaseDate = DateTime.Today.AddMinutes(1);
					resetDates = true;
					session.Store(item);
					session.SaveChanges();
				}
				else
				{
					var sessionItem = session.Load<Bageller>(item.Id);
					sessionItem.InjectFrom(item);	//Doing an injection of values because if we just overwrite it, we lose the connection to the session
					session.SaveChanges();
				}
			}
			if (resetDates) item.NextPurchaseDate = ResetNextPurchaseDates();
			return item;
		}
		public Bageller Delete(int id)
		{
			Bageller item = null;
			using (var session = _documentStore.OpenSession())
			{
				item = session.Load<Bageller>("bagellers/" + id);
				session.Delete(item);
				session.SaveChanges();
			}
			//Might have deleted a date in middle of sequence, and since we don't want a week without bagels, reset the dates
			item.NextPurchaseDate = ResetNextPurchaseDates();
			return item;
		}

		/// <summary>
		/// Resets all NextPurchaseDates so they are incremental, adding weeks if necessary
		/// </summary>
		/// <returns>New start date</returns>
		public DateTime ResetNextPurchaseDates(int addedWeeks = 0)
		{
			var dayOfWeek = (int)DateTime.Today.DayOfWeek;
			//Get the start of new purchase date sequence
			var startDate = DateTime.Today
				.AddDays((dayOfWeek >= 4 ? 11 : 4) - dayOfWeek)
				.AddDays(addedWeeks*7);
			using (var session = _documentStore.OpenSession())
			{
				var upcomingBagellers = (from bageller in session.Query<Bageller>()
				                         orderby bageller.NextPurchaseDate ascending
				                         where bageller.NextPurchaseDate > DateTime.Today
				                         select bageller).ToList();

				for (var i = 0; i < upcomingBagellers.Count(); i++)
				{
					upcomingBagellers.ElementAt(i).NextPurchaseDate = startDate.AddDays(7*i);
				}
				session.SaveChanges();
			}
			return startDate;
		}
	}
}