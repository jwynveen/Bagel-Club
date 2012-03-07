using System.Collections.Generic;
using System.Linq;
using BagelClub.Models;
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

		Bageller Save(Bageller item);
		bool Delete(Bageller item);
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
				                select bageller).Take(1).SingleOrDefault();
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
			using (var session = _documentStore.OpenSession())
			{
				if (item.BagellerId == 0)
				{
					//item.Id = GetNextId()
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
			return item;
		}
		public bool Delete(Bageller item)
		{
			return false;
			/*bool success;
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					success = connection.Delete(item, transaction);
					transaction.Commit();
				}
				connection.Close();
			}
			return success;*/
		}
	}
}