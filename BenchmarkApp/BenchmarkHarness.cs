using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkApp
{
    [HtmlExporter]
    public class BenchmarkHarness
    {
        [Params(100)]
        public int IterationCount;
        private readonly ApiList _apiList = new ApiList();
        [Benchmark]
        public async Task GetCustomerList()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.GetCustomerList();
            }
        }
        [Benchmark]
        public async Task GetCustomerListByCustom()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.GetCustomerListByCustom();
            }
        }
    }
}
