using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkApp
{
    public class ApiList
    {
        public async Task GetCustomerList()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44324/Customer?includeSalesOrderHeaders=true&includeAddresses=true&$filter=customerid eq 29877");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        public async Task GetCustomerListByDapper()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44324/Customer/GetDapper?includeSalesOrderHeaders=true&includeAddresses=true&$filter=customerid eq 29877");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
