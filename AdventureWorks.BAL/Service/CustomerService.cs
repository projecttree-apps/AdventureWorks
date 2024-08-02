using AdventureWorks.BAL.IService;
using AdventureWorks.BAL.ResponseModel;
using AdventureWorks.DAL.Data;
using AdventureWorks.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.BAL.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly dbContext context;

        public CustomerService(dbContext context)
        {
            this.context = context;
        }

        public IQueryable<CustomerResponse> Get()
        {
            var result = context.Customers
                 .Include(customeraddresses => customeraddresses.CustomerAddresses)
                 .Select(data =>
                 new CustomerResponse
                 {
                     CustomerId = data.CustomerId,
                     NameStyle = data.NameStyle,
                     Title = data.Title,
                     FirstName = data.FirstName,
                     MiddleName = data.MiddleName,
                     LastName = data.LastName,
                     Suffix = data.Suffix,
                     CompanyName = data.CompanyName,
                     SalesPerson = data.SalesPerson,
                     EmailAddress = data.EmailAddress,
                     Phone = data.Phone,
                     PasswordHash = data.PasswordHash,
                     PasswordSalt = data.PasswordSalt,
                     Rowguid = data.Rowguid,
                     ModifiedDate = data.ModifiedDate,
                     CustomerAddresses = data.CustomerAddresses.Select(ca => new CustomerAddressResponse
                     {
                         CustomerId = ca.CustomerId,
                         AddressId = ca.AddressId,

                         AddressLine1 = ca.Address.AddressLine1,
                         AddressLine2 = ca.Address.AddressLine2,
                         City = ca.Address.City,
                         StateProvince = ca.Address.StateProvince,
                         CountryRegion = ca.Address.CountryRegion,
                         PostalCode = ca.Address.PostalCode,
                         Rowguid = ca.Address.Rowguid,
                         ModifiedDate = ca.Address.ModifiedDate
                     }).ToList(),
                     SalesOrderHeaders = data.SalesOrderHeaders.Select(soh => new SalesOrderHeaderResponse
                     {
                         SalesOrderId = soh.SalesOrderId,
                         RevisionNumber = soh.RevisionNumber,
                         OrderDate = soh.OrderDate,
                         DueDate = soh.DueDate,
                         ShipDate = soh.ShipDate,
                         Status = soh.Status,
                         OnlineOrderFlag = soh.OnlineOrderFlag,
                         SalesOrderNumber = soh.SalesOrderNumber,
                         PurchaseOrderNumber = soh.PurchaseOrderNumber,
                         AccountNumber = soh.AccountNumber,
                         CustomerId = soh.CustomerId,
                         ShipToAddressId = soh.ShipToAddressId,
                         BillToAddressId = soh.BillToAddressId,
                         ShipMethod = soh.ShipMethod,
                         CreditCardApprovalCode = soh.CreditCardApprovalCode,
                         SubTotal = soh.SubTotal,
                         TaxAmt = soh.TaxAmt,
                         Freight = soh.Freight,
                         TotalDue = soh.TotalDue,
                         Comment = soh.Comment,
                         Rowguid = soh.Rowguid,
                         ModifiedDate = soh.ModifiedDate,
                         SalesOrderDetails = soh.SalesOrderDetails.Select(sod => new SalesOrderDetailResponse
                         {
                             SalesOrderId = sod.SalesOrderId,
                             SalesOrderDetailId = sod.SalesOrderDetailId,
                             OrderQty = sod.OrderQty,
                             ProductId = sod.ProductId,
                             UnitPrice = sod.UnitPrice,
                             UnitPriceDiscount = sod.UnitPriceDiscount,
                             LineTotal = sod.LineTotal,
                             Rowguid = sod.Rowguid,
                             ModifiedDate = sod.ModifiedDate,
                             Product = new ProductResponse
                             {
                                 ProductId = sod.Product.ProductId,
                                 Name = sod.Product.Name,
                                 ProductNumber = sod.Product.ProductNumber,
                                 Color = sod.Product.Color,
                                 StandardCost = sod.Product.StandardCost,
                                 ListPrice = sod.Product.ListPrice,
                                 Size = sod.Product.Size,
                                 Weight = sod.Product.Weight,
                                 ProductCategory = sod.Product.ProductCategory.Name,
                                 ProductModel = sod.Product.ProductModel.Name,
                                 SellStartDate = sod.Product.SellStartDate,
                                 SellEndDate = sod.Product.SellEndDate,
                                 DiscontinuedDate = sod.Product.DiscontinuedDate,
                                 Rowguid = sod.Product.Rowguid,
                                 ModifiedDate = sod.Product.ModifiedDate
                             }
                         }).ToList()
                     }).ToList()
                 }
                 ).AsQueryable();
            return result;
        }
    }
}
