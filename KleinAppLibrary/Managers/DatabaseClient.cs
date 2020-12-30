using Dapper;
using KleinMapLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KleinMapLibrary.Managers
{
    public class DatabaseClient : IDatabaseClient
    {
        private readonly IConfiguration _configuration;
        public DatabaseClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<T>> ExecuteSelectQuery<T>(Expression<Func<T, bool>> predicate, string query)
        {
            string connectionString = _configuration.GetConnectionString("KleinMapDB");
            IQueryable<T> output;

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var x = await cnn.QueryAsync<T>(query);
                output = x.ToList().AsQueryable();
            }

            return output.Where(predicate);
        }

        public async Task<int> ExecuteModifyQuery(string query)
        {
            string connectionString = _configuration.GetConnectionString("KleinMapDB");
            int output;

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                output = await cnn.ExecuteAsync(query);
            }

            return output;
        }

        public async Task<T> ExecuteSelectQuery<T>(string query)
        {
            string connectionString = _configuration.GetConnectionString("KleinMapDB");
            T output;

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var x = await cnn.QueryAsync<T>(query);
                output = x.First();
            }

            return output;
        }
    }


}
