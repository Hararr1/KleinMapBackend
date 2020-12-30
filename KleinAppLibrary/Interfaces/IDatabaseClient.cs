using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace KleinMapLibrary.Interfaces
{
    public interface IDatabaseClient
    {
        public Task<IEnumerable<T>> ExecuteSelectQuery<T>(Expression<Func<T, bool>> predicate, string query);
        public Task<T> ExecuteSelectQuery<T>(string query);
        public Task<int> ExecuteModifyQuery(string query);
    }
}
