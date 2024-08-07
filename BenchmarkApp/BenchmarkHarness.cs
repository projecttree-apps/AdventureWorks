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
        public async Task scenario1stEntity()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.scenario1stEntity();
            }
        }
        [Benchmark]
        public async Task scenario1stDapper()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.scenario1stDapper();
            }
        }
        [Benchmark]
        public async Task scenario2ndEntity()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.scenario2ndEntity();
            }
        }
        [Benchmark]
        public async Task scenario2ndDapper()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.scenario2ndDapper();
            }
        }
    }
}
