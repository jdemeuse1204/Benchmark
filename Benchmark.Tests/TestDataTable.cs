using Benchmark.Tests.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Tests
{
    public static class TestDataTable
    {
        public static DataTable Mock(int maxRows)
        {
            var data = new DataTable();
            var rnd = new Random();

            var properties = typeof(TestRow).GetProperties().ToList();

            foreach (var property in properties)
            {
                data.Columns.Add(property.Name, property.PropertyType);
            }

            for (var i = 0; i < maxRows; i++)
            {
                data.Rows.Add(new object[]
                {
                    rnd.Next(0, 2000000),
                    DateTime.Now,
                    rnd.Next(100000, 2000000),
                    rnd.Next(0, 239048).ToString(),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000),
                    rnd.Next(0, 2000000)
                });
            }

            return data;
        }
    }
}
