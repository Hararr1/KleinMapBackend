using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace KleinMapLibrary.Interfaces
{
    public interface IDatabaseClient
    {
        public IEnumerable<T> ExecuteSelectQuery<T>(Expression<Func<T, bool>> predicate, string query);
    }
}
