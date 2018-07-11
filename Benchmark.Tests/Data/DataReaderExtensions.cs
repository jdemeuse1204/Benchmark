using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Tests.Data
{
    public static class DataReaderExtensions
    {
        public static IFluentMapDataReader<T> MapTo<T>(this IDataReader reader) where T : class, new()
        {
            return new FluentMapDataReader<T>(reader);
        }

        public static dynamic Get(this IDataReader reader, string dbColumnName)
        {
            object value = reader[dbColumnName];

            if (value is DBNull) { return null; }

            return value;
        }
    }
}
