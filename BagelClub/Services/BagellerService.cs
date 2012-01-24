using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using BagelClub.Models;
using Dapper;
using DapperExtensions;

namespace BagelClub.Services
{
	public class BagellerService
	{
		private readonly string _connectionString;
		public BagellerService()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
		}

		public IEnumerable<Bageller> FetchAll()
		{
			IEnumerable<Bageller> items;
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					items = connection.Query<Bageller>("select * from Bageller order by NextPurchaseDate asc", null, transaction);
				}
				connection.Close();
			}
			return items;
		}

		public Bageller GetLastBageller()
		{
			Bageller lastBageller;
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					lastBageller = connection.Query<Bageller>("select top 1 * from Bageller order by NextPurchaseDate desc", null, transaction).First();
				}
				connection.Close();
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