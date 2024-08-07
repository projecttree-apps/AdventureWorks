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
        //[Benchmark]
        //public async Task scenario1stEntity()
        //{
        //    for (int i = 0; i < IterationCount; i++)
        //    {
        //        await _apiList.GetExecute("https://localhost:44324/Customer?$filter=SalesPerson eq 'adventure-works\\shu0'");
        //    }
        //}
        [Benchmark]
        public async Task scenario1stDapper()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.GetExecute("https://localhost:44324/Customer/GetFromSqlRaw?$filter=SalesPerson eq 'adventure-works\\shu0'");
            }
        }
        //[Benchmark]
        //public async Task scenario2ndEntity()
        //{
        //    for (int i = 0; i < IterationCount; i++)
        //    {
        //        await _apiList.GetExecute("https://localhost:44324/Customer?includeAddresses=true&$filter=CustomerAddresses/any(address: address/CountryRegion eq 'United States') and CustomerAddresses/any(address: address/StateProvince eq 'Texas')");
        //    }
        //}
        [Benchmark]
        public async Task scenario2ndDapper()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.GetExecute("https://localhost:44324/Customer/GetFromSqlRaw?includeAddresses=true&$filter=CustomerAddresses/any(address: address/CountryRegion eq 'United States') and CustomerAddresses/any(address: address/StateProvince eq 'Texas')");
            }
        }
        //[Benchmark]
        //public async Task scenario3rdEntity()
        //{
        //    for (int i = 0; i < IterationCount; i++)
        //    {
        //        await _apiList.GetExecute("https://localhost:44324/Customer?includeSalesOrderHeaders=true&$filter=SalesOrderHeaders/any(soh: soh/SalesOrderDetails/any(sod: sod/Product/Name eq 'Sport-100 Helmet, Red'))&$orderby=customerId asc");
        //    }
        //}
        [Benchmark]
        public async Task scenario3rdDapper()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.GetExecute("https://localhost:44324/Customer/GetFromSqlRaw?includeSalesOrderHeaders=true&$filter=SalesOrderHeaders/any(soh: soh/SalesOrderDetails/any(sod: sod/Product/Name eq 'Sport-100 Helmet, Red'))&$orderby=customerId asc");
            }
        }
        //[Benchmark]
        //public async Task scenario4thEntity()
        //{
        //    for (int i = 0; i < IterationCount; i++)
        //    {
        //        await _apiList.GetExecute("https://localhost:44324/Customer?$filter=SalesOrderHeaders/any(soh: soh/SalesOrderDetails/any(sod: sod/Product/ProductCategory eq 'Helmets'))&includeSalesOrderHeaders=true");
        //    }
        //}
        [Benchmark]
        public async Task scenario4thDapper()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await _apiList.GetExecute("https://localhost:44324/Customer/GetFromSqlRaw?$filter=SalesOrderHeaders/any(soh: soh/SalesOrderDetails/any(sod: sod/Product/ProductCategory eq 'Helmets'))&includeSalesOrderHeaders=true");
            }
        }
    }
}
