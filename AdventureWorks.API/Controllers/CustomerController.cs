using AdventureWorks.BAL.IService;
using AdventureWorks.BAL.ResponseModel;
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
        public ActionResult<IEnumerable<CustomerResponse>> Get()
        {
            var response = customerService.Get();
            return Ok(response);
        }
    }
}
