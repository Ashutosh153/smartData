using App.Core.Model;
using Core.Interface;
using Dapper;
using Domain.User;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Command
{
    public class AddToSalesMasterCommand:IRequest<ResponceDto>
    {
        public SalesMasterDto SalesMasterVal { get; set; }
    }
    public class AddToSalesMasterCommandHandler : IRequestHandler<AddToSalesMasterCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public AddToSalesMasterCommandHandler(IAppDbContext appDbContext,IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }
        public async Task<ResponceDto> Handle(AddToSalesMasterCommand request, CancellationToken cancellationToken)
        {



            var salesEntry = request.SalesMasterVal.Adapt<SalesMaster>();

           
            var Maxid =await _appDbContext.Set<SalesMaster>()
                         .Select(e => e.Id)
                         .DefaultIfEmpty()
                         .MaxAsync();

            salesEntry.InvoiceId = "ORD-" + (Maxid + 1).ToString().PadLeft(3, '0');
            salesEntry.InvoiceDate = DateTime.Today;

            await _appDbContext.Set<SalesMaster>().AddAsync(salesEntry);
            await _appDbContext.SaveChangesAsync();





            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);

            const string sql = @"
                          SELECT 
cd.Id AS CartDetailsId,
cd.CartId,
cd.ProductId,
cd.Qty AS quantity,
p.ProductName AS ProductName,
p.Category AS Category,
p.Brand AS Brand,
p.SellingPrice AS Price,
p.ProductImage AS ImageUrl,
p.Stock as Stock
FROM CartMaster cm
INNER JOIN CartDetail cd ON cm.Id = cd.CartId
INNER JOIN products p ON cd.ProductId = p.Id
WHERE cm.UserId = @userId
AND p.IsDeleted = 0";

            var parameters = new { UserId = request.SalesMasterVal.UserId };



            // Execute the query using Dapper
            var cartDetails = await connection.QueryAsync<CartProductDto>(sql, parameters);

            var cartdetailsList=cartDetails.ToList();

            foreach (var product in cartdetailsList) 
            {
                var productValue = await _appDbContext.Set<Product>()
                    .FirstOrDefaultAsync((x) => x.Id == product.ProductId);


                SalesDetail salesDetailValues = new SalesDetail()
                {
                    InvoiceId = salesEntry.Id,
                    ProductId = productValue.Id,
                    ProductCode=productValue.ProductCode,
                    SaleQty=product.Quantity,
                    SellingPrice=(float)product.Price


                };
                 await _appDbContext.Set<SalesDetail>().AddAsync(salesDetailValues);





                await _appDbContext.SaveChangesAsync();


            }

                var    cartIDVal = await _appDbContext.Set<CartMaster>().FirstOrDefaultAsync((x) => x.UserId == request.SalesMasterVal.UserId);

            if(cartIDVal!=null)
            {
                const string sqllDeleteCartContent = @"
                                                    delete from cartDetail where CartId=@cartID
                                                        ";
                var parameters1 = new { cartID =cartIDVal.Id };
                var i= await connection.QueryAsync(sqllDeleteCartContent, parameters1);

                await _appDbContext.SaveChangesAsync();
            }



            return new ResponceDto
            {
                isSuccess = true,
                data=salesEntry.InvoiceId

            };
        }
    }
}
