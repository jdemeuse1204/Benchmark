using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Tests.Data
{
    internal class DataLoader
    {
        protected static T Load<T>(IDataReader reader, Dictionary<string, string> propertyColumnMap, Dictionary<string, object> propertyConversion, Dictionary<string, object> defaultValues) where T : class, new()
        {
            var result = new T();

            foreach (var property in propertyColumnMap)
            {
                dynamic value = reader.Get(property.Value);

                try
                {
                    object defaultValue = defaultValues[property.Key];
                    dynamic converter = propertyConversion[property.Key];

                    SetPropertyValue(result, property.Key, converter == null ? value ?? defaultValue : (value == null ? defaultValue : converter((dynamic)value)));
                }
                catch (ArgumentException ex)
                {
                    // Rethrow ArgumentExceptions and add on more information so the error makes sense, otherwise
                    // users will not know which property is the culprit
                    throw new ArgumentException($"{ex.Message}  Property Name: {property.Key}, Database Colum Name: {property.Value}", ex.InnerException);
                }
                catch (RuntimeBinderException ex)
                {
                    // Rethrow RuntimeBinderException and add on more information so the error makes sense, otherwise
                    // users will not know which property is the culprit
                    throw new RuntimeBinderException($"{ex.Message}  Database type does not match type in Convert function.  Property Name: {property.Key}, Database Column Name: {property.Value}, Database Type: {value.GetType().Name}", ex.InnerException);
                }
            }

            return result;
        }

        protected static string GetPropertyNameFromExpression<T>(Expression<Func<T, string>> expression)
        {
            try
            {
                // we let runtime choose the method to run
                return GetPropertyName(expression.Body as dynamic);
            }
            catch
            {
                throw new Exception("Expression must be member Expression.  Ex - .Method(w => w.PropertyName)"); ;
            }
        }

        protected static string GetPropertyNameFromExpression<T, K>(Expression<Func<T, K>> expression) where K : struct
        {
            try
            {
                // we let runtime choose the method to run
                return GetPropertyName(expression.Body as dynamic);
            }
            catch
            {
                throw new Exception("Expression must be member Expression.  Ex - .Method(w => w.PropertyName)"); ;
            }
        }

        protected static string GetPropertyNameFromExpression<T, K>(Expression<Func<T, K?>> expression) where K : struct
        {
            try
            {
                // we let runtime choose the method to run
                return GetPropertyName(expression.Body as dynamic);
            }
            catch
            {
                throw new Exception("Expression must be member Expression.  Ex - .Method(w => w.PropertyName)"); ;
            }
        }

        protected static string GetPropertyNameFromExpression<T>(Expression<Func<T, object>> expression)
        {
            try
            {
                // we let runtime choose the method to run
                return GetPropertyName(expression.Body as dynamic);
            }
            catch
            {
                throw new Exception("Expression must be member Expression.  Ex - .Method(w => w.PropertyName)"); ;
            }
        }

        protected static PropertyInfo GetPropertyInfoFromPropertyName<T>(Expression<Func<T, string>> expression)
        {
            return GetPropertyInfo(expression.Body as dynamic);
        }

        protected static PropertyInfo GetPropertyInfoFromPropertyName<T, K>(Expression<Func<T, K?>> expression) where K : struct
        {
            return GetPropertyInfo(expression.Body as dynamic);
        }

        protected static PropertyInfo GetPropertyInfoFromPropertyName<T, K>(Expression<Func<T, K>> expression) where K : struct
        {
            return GetPropertyInfo(expression.Body as dynamic);
        }

        #region Helpers
        private static void SetPropertyValue(object entity, string columnName, object value)
        {
            entity.GetType().GetProperty(columnName).SetValue(entity, value);
        }

        private static PropertyInfo GetPropertyInfo(UnaryExpression expression)
        {
            return GetPropertyInfo((MemberExpression)expression.Operand);
        }

        private static PropertyInfo GetPropertyInfo(MemberExpression expression)
        {
            return (PropertyInfo)expression.Member;
        }

        private static string GetPropertyName(UnaryExpression expression)
        {
            return GetPropertyName((MemberExpression)expression.Operand);
        }

        private static string GetPropertyName(MemberExpression expression)
        {
            return expression.Member.Name;
        }
        #endregion
    }
}
