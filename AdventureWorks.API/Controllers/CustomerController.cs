using AdventureWorks.BAL.IService;
using AdventureWorks.BAL.ResponseModel;
using AdventureWorks.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AdventureWorks.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ODataController
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 4, MaxAnyAllExpressionDepth = 4)]
        public ActionResult<dynamic> Get(bool includeAddresses, bool includeSalesOrderHeaders)
        {
            var response = customerService.Get(includeAddresses, includeSalesOrderHeaders);
            return Ok(response);
        }

        [HttpGet("GetDapper")]
        [EnableQuery(MaxExpansionDepth = 4, MaxAnyAllExpressionDepth = 4)]
        public ActionResult<dynamic> GetDapper(ODataQueryOptions<dynamic> queryOptions, bool includeAddresses = false, bool includeSalesOrderHeaders = false)
        {
            var response = customerService.GetDapper(queryOptions.Filter?.RawValue, queryOptions.OrderBy?.RawValue, includeAddresses, includeSalesOrderHeaders);
            return Ok(response);
        }
    }
}
