using App.Core.Model;
using Core.Interface;
using Dapper;
using Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class GetCartQuantityQuerry:IRequest<ResponceDto>
    {
        public int userId {  get; set; }
    }
    public class GetCartQuantityQuerryHamdler : IRequestHandler<GetCartQuantityQuerry, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public GetCartQuantityQuerryHamdler(IAppDbContext appDbContext,IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        public async Task<ResponceDto> Handle(GetCartQuantityQuerry request, CancellationToken cancellationToken)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);


            var cartId = await _appDbContext.Set<CartMaster>()
                .FirstOrDefaultAsync((x) => x.UserId == request.userId);

            if (cartId == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    data=0

                };
            }

            const string sql = @"
                               SELECT COUNT(DISTINCT cd.ProductId) AS ProductCount
                        FROM cartMaster cm
                        JOIN cartDetail cd ON cm.Id = cd.CartId
                        WHERE cm.UserId = @UserId
                                            ";

            var parameters = new { UserId = request.userId };

            // Execute the query using Dapper
            var cartItemCount = await connection.QueryAsync<int>(sql, parameters);

            return new ResponceDto
            {
                isSuccess = true,
                data = cartItemCount
            };
            


            //throw new NotImplementedException();
        }
    }
}
