using Dapper;
using KleinMapLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;

namespace KleinMapLibrary.Managers
{
    public class DatabaseClient : IDatabaseClient
    {
        private readonly IConfiguration _configuration;
        public DatabaseClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<T> ExecuteSelectQuery<T>(Expression<Func<T, bool>> predicate, string query)
        {
            string connectionString = _configuration.GetConnectionString("KleinMapDB");
            IQueryable<T> output;

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                output = cnn.Query<T>(query).ToList().AsQueryable();
            }

            return output.Where(predicate);
        }
    }


}
