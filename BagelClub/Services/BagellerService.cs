using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using BagelClub.Models;
using Dapper;

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
	}
}