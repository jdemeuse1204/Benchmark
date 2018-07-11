using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Tests.Data
{
    public interface IFluentMapDataReader<T> where T : class, new()
    {
        IFluentMapDataReader<T> Remap<K>(Expression<Func<T, K>> expression, string dbColumnName) where K : struct;
        IFluentMapDataReader<T> Remap<K>(Expression<Func<T, K?>> expression, string dbColumnName) where K : struct;
        IFluentMapDataReader<T> Remap(Expression<Func<T, string>> expression, string dbColumnName);
        IFluentMapDataReader<T> Skip(Expression<Func<T, object>> expression);
        IFluentMapDataReader<T> DefaultIfNull<K>(Expression<Func<T, K>> expression, K defaultValue) where K : struct;
        IFluentMapDataReader<T> DefaultIfNull<K>(Expression<Func<T, K?>> expression, K defaultValue) where K : struct;
        IFluentMapDataReader<T> DefaultIfNull(Expression<Func<T, string>> expression, string defaultValue);
        IFluentMapDataReader<T> Convert<K>(Expression<Func<T, K>> expression) where K : struct;
        IFluentMapDataReader<T> Convert<K>(Expression<Func<T, K?>> expression) where K : struct;
        IFluentMapDataReader<T> Convert<K, Z>(Expression<Func<T, K>> expression, Func<Z, K> handler) where K : struct;
        IFluentMapDataReader<T> Convert<K, Z>(Expression<Func<T, K?>> expression, Func<Z, K> handler) where K : struct;
        IFluentMapDataReader<T> Convert(Expression<Func<T, string>> expression);
        IFluentMapDataReader<T> Convert<Z>(Expression<Func<T, string>> expression, Func<Z, string> handler);
        T LoadOne();
        List<T> LoadAll();
    }
}
