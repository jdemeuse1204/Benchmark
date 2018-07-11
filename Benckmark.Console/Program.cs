using Benchmark;
using Benchmark.Tests;
using Benchmark.Tests.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benckmark.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://stackoverflow.com/questions/1047218/benchmarking-small-code-samples-in-c-can-this-implementation-be-improved

            //Accessor
            //https://stackoverflow.com/questions/6158768/c-sharp-reflection-fastest-way-to-update-a-property-value
            var table = TestDataTable.Mock(2000);

            var dataLoaderResult = Profiler.Profile("Data Load Test - Fluent Reader", 1, () => 
            {
                using (IDataReader reader = table.CreateDataReader())
                {
                    reader.MapTo<TestRow>().LoadOne();
                }
            });

            var noLoaderResult = Profiler.Profile("Data Load Test - No Loader", 1, () =>
            {
                using (IDataReader reader = table.CreateDataReader())
                {
                    reader.Read();

                    var x = new TestRow
                    {
                        One = (int)reader["One"]
                    };
                }
            });

            if (dataLoaderResult != null && noLoaderResult != null)
            {

            }
        }
    }


}
