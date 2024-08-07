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
        public async Task scenario1stEntity()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44324/Customer?$filter=SalesPerson eq 'adventure-works\\shu0'");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        public async Task scenario1stDapper()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44324/Customer/GetCustom?$filter=SalesPerson eq 'adventure-works\\shu0'");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

        }
        public async Task scenario2ndEntity()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44324/Customer?includeAddresses=true&$filter=CustomerAddresses/any(address: address/CountryRegion eq 'United States') and CustomerAddresses/any(address: address/StateProvince eq 'Texas')");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        public async Task scenario2ndDapper()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44324/Customer/GetCustom?includeAddresses=true&$filter=CustomerAddresses/any(address: address/CountryRegion eq 'United States') and CustomerAddresses/any(address: address/StateProvince eq 'Texas')");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

        }
    }
}
