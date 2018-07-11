using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Tests.Data
{
    internal class FluentMapDataReader<T> : DataLoader, IFluentMapDataReader<T> where T : class, new()
    {
        #region Properties and Fields
        private readonly IDataReader _reader;
        private readonly Dictionary<string, string> _propertyColumnMap;
        private readonly Dictionary<string, object> _propertyConversionMap;
        private readonly Dictionary<string, object> _defaultValues;
        private readonly List<PropertyInfo> _properties;
        #endregion

        #region Constructor
        public FluentMapDataReader(IDataReader reader)
        {
            _reader = reader;
            _properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty).Where(w => w.CanWrite).ToList();
            _propertyConversionMap = CreateConversionMap();
            _propertyColumnMap = CreateMap();
            _defaultValues = CreateDefaultValuesMap();
        }
        #endregion

        #region Remap
        public IFluentMapDataReader<T> Remap<K>(Expression<Func<T, K>> expression, string dbColumnName) where K : struct
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _propertyColumnMap[propertyName] = dbColumnName;
            return this;
        }

        public IFluentMapDataReader<T> Remap<K>(Expression<Func<T, K?>> expression, string dbColumnName) where K : struct
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _propertyColumnMap[propertyName] = dbColumnName;
            return this;
        }

        public IFluentMapDataReader<T> Remap(Expression<Func<T, string>> expression, string dbColumnName)
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _propertyColumnMap[propertyName] = dbColumnName;
            return this;
        }
        #endregion

        #region Skip
        public IFluentMapDataReader<T> Skip(Expression<Func<T, object>> expression)
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _propertyColumnMap.Remove(propertyName);
            return this;
        }
        #endregion

        #region Convert
        public IFluentMapDataReader<T> Convert<K>(Expression<Func<T, K>> expression) where K : struct
        {
            PropertyInfo memberInfo = GetPropertyInfoFromPropertyName(expression);
            TypeConverter converter = TypeDescriptor.GetConverter(memberInfo.PropertyType);
            Func<object, object> convertFunc = (data) => converter.ConvertFrom(data.ToString());
            _propertyConversionMap[memberInfo.Name] = convertFunc;
            return this;
        }

        public IFluentMapDataReader<T> Convert<K>(Expression<Func<T, K?>> expression) where K : struct
        {
            PropertyInfo memberInfo = GetPropertyInfoFromPropertyName(expression);
            TypeConverter converter = TypeDescriptor.GetConverter(memberInfo.PropertyType);
            Func<object, object> convertFunc = (data) => converter.ConvertFrom(data.ToString());
            _propertyConversionMap[memberInfo.Name] = convertFunc;
            return this;
        }

        public IFluentMapDataReader<T> Convert<K, Z>(Expression<Func<T, K>> expression, Func<Z, K> handler) where K : struct
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _propertyConversionMap[propertyName] = handler;
            return this;
        }

        public IFluentMapDataReader<T> Convert<K, Z>(Expression<Func<T, K?>> expression, Func<Z, K> handler) where K : struct
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _propertyConversionMap[propertyName] = handler;
            return this;
        }

        public IFluentMapDataReader<T> Convert(Expression<Func<T, string>> expression)
        {
            PropertyInfo memberInfo = GetPropertyInfoFromPropertyName(expression);
            TypeConverter converter = TypeDescriptor.GetConverter(memberInfo.PropertyType);
            Func<object, object> convertFunc = (data) => converter.ConvertFrom(data.ToString());
            _propertyConversionMap[memberInfo.Name] = convertFunc;
            return this;
        }

        public IFluentMapDataReader<T> Convert<Z>(Expression<Func<T, string>> expression, Func<Z, string> handler)
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _propertyConversionMap[propertyName] = handler;
            return this;
        }
        #endregion

        #region Default If Null
        public IFluentMapDataReader<T> DefaultIfNull<K>(Expression<Func<T, K>> expression, K defaultValue) where K : struct
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _defaultValues[propertyName] = defaultValue;
            return this;
        }

        public IFluentMapDataReader<T> DefaultIfNull<K>(Expression<Func<T, K?>> expression, K defaultValue) where K : struct
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _defaultValues[propertyName] = defaultValue;
            return this;
        }

        public IFluentMapDataReader<T> DefaultIfNull(Expression<Func<T, string>> expression, string defaultValue)
        {
            string propertyName = GetPropertyNameFromExpression(expression);
            _defaultValues[propertyName] = defaultValue;
            return this;
        }
        #endregion

        #region Loading
        public T LoadOne()
        {
            if (!_reader.Read()) { return default(T); }

            T result = Load<T>(_reader, _propertyColumnMap, _propertyConversionMap, _defaultValues);

            return result;
        }

        public List<T> LoadAll()
        {
            var result = new List<T>();

            while (_reader.Read()) { result.Add(Load<T>(_reader, _propertyColumnMap, _propertyConversionMap, _defaultValues)); }

            return result;
        }
        #endregion

        #region Private Methods
        private Dictionary<string, string> CreateMap()
        {
            return _properties.ToDictionary(w => w.Name, x => x.Name);
        }

        private Dictionary<string, object> CreateConversionMap()
        {
            var result = new Dictionary<string, object>();
            foreach (PropertyInfo item in _properties) { result.Add(item.Name, null); }
            return result;
        }

        private Dictionary<string, object> CreateDefaultValuesMap()
        {
            var result = new Dictionary<string, object>();
            foreach (PropertyInfo item in _properties) { result.Add(item.Name, null); }
            return result;
        }
        #endregion
    }
}
