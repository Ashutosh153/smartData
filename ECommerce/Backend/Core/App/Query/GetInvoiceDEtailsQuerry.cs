using App.Core.Model;
using Core.Interface;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class GetInvoiceDEtailsQuerry:IRequest<ResponceDto>
    {
        public string invoiceId {  get; set; }  
    }
    public class GetInvoiceDEtailsQuerryHandler : IRequestHandler<GetInvoiceDEtailsQuerry, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public GetInvoiceDEtailsQuerryHandler(IAppDbContext appDbContext,IConfiguration configuration)
        {
               _appDbContext = appDbContext;
            _configuration = configuration;
        }
        public async Task<ResponceDto> Handle(GetInvoiceDEtailsQuerry request, CancellationToken cancellationToken)
        {

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);

            const string sql = @"
                            select 
                            sm.InvoiceId as BillId,
                            c.Name as Country,
                            s.name as State,
                            sm.Subtotal as Total,
                            sm.InvoiceDate as Date,
                            sd.ProductId as ProductId,
                            p.ProductName as ProductName,
                            sd.SaleQty as quentity,
                            sd.SellingPrice as price,
                            u.FirstName as FirstName,
                            u.LastName as LastName
                            from salesMasters sm
                            left join salesDetails sd on sm.Id=sd.InvoiceId
                            inner join Country c on c.Id=sm.DeliveryCountry
                            inner join State s on s.Id=sm.DeliveryState
                            inner join Users u on u.Id=sm.UserId
                            inner join products p on sd.ProductId=p.Id
                            where sm.InvoiceId=@invoiceId";

            var parameters = new { invoiceId = request.invoiceId };

            // Execute the query using Dapper
            var cartDetails = await connection.QueryAsync<InvoiceDataDto>(sql, parameters);



            


            if (cartDetails != null)
            {



                return new ResponceDto
                {
                    isSuccess = true,
                    data = cartDetails
                };
            }
            else
            {
                return new ResponceDto
                {
                    isSuccess = false
                };
            }
           
        }
    }
}
