using AdventureWorks.BAL.IService;
using AdventureWorks.BAL.ResponseModel;
using AdventureWorks.DAL.Data;
using AdventureWorks.DAL.Models;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.BAL.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly dbContext context;
        private readonly IMapper _mapper;
        private readonly IDbConnection _dbConnection;
        public CustomerService(dbContext context, IMapper mapper, IDbConnection dbConnection)
        {
            this.context = context;
            _mapper = mapper;
            _dbConnection = dbConnection;
        }
        public IQueryable<CustomerResponse> Get(bool includeAddresses, bool includeSalesOrderHeaders)
        {
            var result = context.Customers
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
                     CustomerAddresses = includeAddresses ? data.CustomerAddresses.Select(ca => new CustomerAddressResponse
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
                     }).ToList() : new List<CustomerAddressResponse>(),
                     SalesOrderHeaders = includeSalesOrderHeaders ? data.SalesOrderHeaders.Select(soh => new SalesOrderHeaderResponse
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
                         SalesOrderDetails = (soh.SalesOrderDetails.Select(sod => new SalesOrderDetailResponse
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
                         }).ToList())

                     }).ToList() : new List<SalesOrderHeaderResponse>()
                 }
                 ).AsQueryable();
            return result;
        }

        public dynamic GetDapper(string filter, string orderBy, bool includeAddresses, bool includeSalesOrderHeaders)
        {
            var sqlQuery = BuildSqlQuery(filter, orderBy, includeAddresses, includeSalesOrderHeaders);

            var customerDictionary = new Dictionary<int, CustomerResponseCustom>();

            var result = _dbConnection.Query<CustomerResponseCustom, CustomerAddressResponse, SalesOrderHeaderResponseCustom, SalesOrderDetailResponse, ProductResponse, CustomerResponseCustom>(
                sqlQuery,
                (customer, address, order, detail, product) =>
                {
                    CustomerResponseCustom customerEntry;

                    if (!customerDictionary.TryGetValue(customer.CustomerId, out customerEntry))
                    {
                        customerEntry = customer;
                        customerEntry.CustomerAddresses = new List<CustomerAddressResponse>();
                        customerEntry.SalesOrderHeaders = new List<SalesOrderHeaderResponseCustom>();
                        customerDictionary.Add(customerEntry.CustomerId, customerEntry);
                    }

                    if (address != null && !customerEntry.CustomerAddresses.Any(a => a.AddressId == address.AddressId))
                    {
                        customerEntry.CustomerAddresses.Add(address);
                    }

                    if (order != null)
                    {
                        var existingOrder = customerEntry.SalesOrderHeaders.FirstOrDefault(o => o.SalesOrderId == order.SalesOrderId);
                        if (existingOrder == null)
                        {
                            order.SalesOrderDetails = new List<SalesOrderDetailResponse>();
                            customerEntry.SalesOrderHeaders.Add(order);
                            existingOrder = order;
                        }

                        if (detail != null)
                        {
                            var existingDetail = existingOrder.SalesOrderDetails.FirstOrDefault(d => d.SalesOrderDetailId == detail.SalesOrderDetailId);
                            if (existingDetail == null)
                            {
                                detail.Product = product;
                                existingOrder.SalesOrderDetails.Add(detail);
                            }
                        }
                    }

                    return customerEntry;
                },
                splitOn: "CustomerId, AddressId, SalesOrderID, SalesOrderDetailId, ProductId"
            ).Distinct().ToList();

            return result;
        }
        private Dictionary<string, string> getCustomerFilterMap()
        {
            var filterMap = new Dictionary<string, string>();
            filterMap.Add("customerid ", "[c].customerid ");
            filterMap.Add("customeraddresses/stateprovince ", "[t].[stateprovince] ");
            return filterMap;

        }
        private string BuildSqlQuery(string filter, string orderBy, bool includeAddresses, bool includeSalesOrderHeaders)
        {
            var addressSeelct = "";
            var addressFrom = "";
            if (includeAddresses)
            {
                addressSeelct = ",[t].[CustomerId], [t].[AddressId], [t].[AddressLine1], [t].[AddressLine2], [t].[City], [t].[StateProvince], [t].[CountryRegion], [t].[PostalCode], [t].[Rowguid], [t].[ModifiedDate], [t].[AddressID0]";

                addressFrom = @"LEFT JOIN (
    SELECT [c0].[CustomerID] AS [CustomerId], [c0].[AddressID] AS [AddressId], [a].[AddressLine1], [a].[AddressLine2], [a].[City], [a].[StateProvince], [a].[CountryRegion], [a].[PostalCode], [a].[rowguid] AS [Rowguid], [a].[ModifiedDate], [a].[AddressID] AS [AddressID0]
    FROM [SalesLT].[CustomerAddress] AS [c0]
    INNER JOIN [SalesLT].[Address] AS [a] ON [c0].[AddressID] = [a].[AddressID]
) AS [t] ON [c].[CustomerID] = [t].[CustomerId]";
            }
            var orderSeelct = "";
            var orderFrom = "";
            if (includeSalesOrderHeaders)
            {
                orderSeelct = ",[t0].[SalesOrderID], [t0].[RevisionNumber], [t0].[OrderDate], [t0].[DueDate], [t0].[ShipDate], [t0].[Status], [t0].[OnlineOrderFlag], [t0].[SalesOrderNumber], [t0].[PurchaseOrderNumber], [t0].[AccountNumber], [t0].[CustomerID], [t0].[ShipToAddressID], [t0].[BillToAddressID], [t0].[ShipMethod], [t0].[CreditCardApprovalCode], [t0].[SubTotal], [t0].[TaxAmt], [t0].[Freight], [t0].[TotalDue], [t0].[Comment], [t0].[rowguid], [t0].[ModifiedDate], [t0].[SalesOrderId0], [t0].[SalesOrderDetailId], [t0].[OrderQty], [t0].[ProductId], [t0].[UnitPrice], [t0].[UnitPriceDiscount], [t0].[LineTotal], [t0].[Rowguid0], [t0].[ModifiedDate0], [t0].[ProductId0], [t0].[Name], [t0].[ProductNumber], [t0].[Color], [t0].[StandardCost], [t0].[ListPrice], [t0].[Size], [t0].[Weight], [t0].[ProductCategory], [t0].[ProductModel], [t0].[SellStartDate], [t0].[SellEndDate], [t0].[DiscontinuedDate], [t0].[Rowguid00], [t0].[ModifiedDate00], [t0].[ProductCategoryID], [t0].[ProductModelID]";

                orderFrom = @"LEFT JOIN (
    SELECT [s].[SalesOrderID], [s].[RevisionNumber], [s].[OrderDate], [s].[DueDate], [s].[ShipDate], [s].[Status], [s].[OnlineOrderFlag], [s].[SalesOrderNumber], [s].[PurchaseOrderNumber], [s].[AccountNumber], [s].[CustomerID], [s].[ShipToAddressID], [s].[BillToAddressID], [s].[ShipMethod], [s].[CreditCardApprovalCode], [s].[SubTotal], [s].[TaxAmt], [s].[Freight], [s].[TotalDue], [s].[Comment], [s].[rowguid], [s].[ModifiedDate], [t1].[SalesOrderId] AS [SalesOrderId0], [t1].[SalesOrderDetailId], [t1].[OrderQty], [t1].[ProductId], [t1].[UnitPrice], [t1].[UnitPriceDiscount], [t1].[LineTotal], [t1].[Rowguid] AS [Rowguid0], [t1].[ModifiedDate] AS [ModifiedDate0], [t1].[ProductId0], [t1].[Name], [t1].[ProductNumber], [t1].[Color], [t1].[StandardCost], [t1].[ListPrice], [t1].[Size], [t1].[Weight], [t1].[ProductCategory], [t1].[ProductModel], [t1].[SellStartDate], [t1].[SellEndDate], [t1].[DiscontinuedDate], [t1].[Rowguid0] AS [Rowguid00], [t1].[ModifiedDate0] AS [ModifiedDate00], [t1].[ProductCategoryID], [t1].[ProductModelID]
    FROM [SalesLT].[SalesOrderHeader] AS [s]
    LEFT JOIN (
        SELECT [s0].[SalesOrderID] AS [SalesOrderId], [s0].[SalesOrderDetailID] AS [SalesOrderDetailId], [s0].[OrderQty], [s0].[ProductID] AS [ProductId], [s0].[UnitPrice], [s0].[UnitPriceDiscount], [s0].[LineTotal], [s0].[rowguid] AS [Rowguid], [s0].[ModifiedDate], [p].[ProductID] AS [ProductId0], [p].[Name], [p].[ProductNumber], [p].[Color], [p].[StandardCost], [p].[ListPrice], [p].[Size], [p].[Weight], [p0].[Name] AS [ProductCategory], [p1].[Name] AS [ProductModel], [p].[SellStartDate], [p].[SellEndDate], [p].[DiscontinuedDate], [p].[rowguid] AS [Rowguid0], [p].[ModifiedDate] AS [ModifiedDate0], [p0].[ProductCategoryID], [p1].[ProductModelID]
        FROM [SalesLT].[SalesOrderDetail] AS [s0]
        INNER JOIN [SalesLT].[Product] AS [p] ON [s0].[ProductID] = [p].[ProductID]
        LEFT JOIN [SalesLT].[ProductCategory] AS [p0] ON [p].[ProductCategoryID] = [p0].[ProductCategoryID]
        LEFT JOIN [SalesLT].[ProductModel] AS [p1] ON [p].[ProductModelID] = [p1].[ProductModelID]
    ) AS [t1] ON [s].[SalesOrderID] = [t1].[SalesOrderId]
) AS [t0] ON [c].[CustomerID] = [t0].[CustomerID]";
            }

            var query1 = $@"
SELECT [c].[CustomerID], [c].[NameStyle], [c].[Title], [c].[FirstName], [c].[MiddleName], [c].[LastName], [c].[Suffix], [c].[CompanyName], [c].[SalesPerson], [c].[EmailAddress], [c].[Phone], [c].[PasswordHash], [c].[PasswordSalt], [c].[rowguid], [c].[ModifiedDate]
{addressSeelct}
{orderSeelct}
FROM [SalesLT].[Customer] AS [c]
{addressFrom}
{orderFrom}
";

            var whereClause = string.IsNullOrEmpty(filter) == true ? string.Empty : $" WHERE {ODataFilterToSql(filter)}";
            var orderByClause = string.IsNullOrEmpty(orderBy) == true ? string.Empty : $" ORDER BY {orderBy}";

            return $"{query1} {whereClause} {orderByClause}";
        }
        private string ODataFilterToSql(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return string.Empty;
            }

            var sqlFilter = filter
                .Replace(" eq ", " = ")
                .Replace(" ne ", " != ")
                .Replace(" gt ", " > ")
                .Replace(" ge ", " >= ")
                .Replace(" lt ", " < ")
                .Replace(" le ", " <= ")
                .Replace("'", "''"); // Handling single quotes for SQL

            foreach (var item in getCustomerFilterMap())
            {
                sqlFilter = sqlFilter.ToLower().Replace(item.Key, item.Value);
            }
            return sqlFilter;
        }

    }
}
