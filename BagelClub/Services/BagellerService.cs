using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using BagelClub.Models;
using Dapper;
using DapperExtensions;
using Raven.Client.Document;

namespace BagelClub.Services
{
	public interface IBagellerService
	{
		IEnumerable<Bageller> FetchAll();
		Bageller GetLastBageller();
		void SetNextPurchaseDate(Bageller nextBageller);

		Bageller Save(Bageller item);
		bool Delete(Bageller item);
	}
	public class BagellerService : IBagellerService
	{
		private readonly string _connectionString;
		private readonly DocumentStore _documentStore;
		public BagellerService()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
			_documentStore = new DocumentStore { ConnectionStringName  = "RavenDB"};
			_documentStore.Initialize();
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
			return item.BagellerId == 0 ? Insert(item) : Update(item);
		}
		public bool Delete(Bageller item)
		{
			return false;
			bool success;
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
			return success;
		}
		private Bageller Insert(Bageller item)
		{
			return null;
			bool success;
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					var result = connection.Insert(item, transaction);
					transaction.Commit();
					success = result >= 1;
					item.BagellerId = result;
				}
				connection.Close();
			}
			return success ? item : null;
		}
		private Bageller Update(Bageller item)
		{
			return null;
			bool success;
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					success = connection.Update(item, transaction);
					transaction.Commit();
				}
				connection.Close();
			}
			return success ? item : null;
		}
	}
}