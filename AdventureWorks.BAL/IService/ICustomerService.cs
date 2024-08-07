using AdventureWorks.BAL.ResponseModel;
using AdventureWorks.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.BAL.IService
{
    public interface ICustomerService
    {
        IQueryable<CustomerResponse> Get(bool includeAddresses, bool includeSalesOrderHeaders);
        Task<List<CustomerResponseCustom>> GetCustomeQuery(string filter, string orderBy, bool includeAddresses, bool includeSalesOrderHeaders);
    }
}
