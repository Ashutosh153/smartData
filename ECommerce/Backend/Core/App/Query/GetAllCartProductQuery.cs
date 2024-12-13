using App.Core.Model;
using Core.Interface;
using Dapper;
using Domain.User;
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
    public class GetAllCartProductQuery:IRequest<ResponceDto>
    {
        public int userId { get; set; }
    }
    public class GetAllCartProductQueryHandler : IRequestHandler<GetAllCartProductQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public GetAllCartProductQueryHandler(IAppDbContext appDbContext,IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }
        public async Task<ResponceDto> Handle(GetAllCartProductQuery request, CancellationToken cancellationToken)
        {
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
      WHERE cm.UserId = @UserId
      AND p.IsDeleted = 0";

            var parameters = new { UserId = request.userId };

            // Execute the query using Dapper
            var cartDetails = await connection.QueryAsync<CartProductDto>(sql, parameters);

            return new ResponceDto
            {
                isSuccess = true,
                data = cartDetails.ToList()
            };

           
        }
    }
}
