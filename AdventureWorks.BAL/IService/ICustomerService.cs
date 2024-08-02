using AdventureWorks.BAL.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.BAL.IService
{
    public interface ICustomerService
    {
        IQueryable<CustomerResponse> Get();
    }
}
