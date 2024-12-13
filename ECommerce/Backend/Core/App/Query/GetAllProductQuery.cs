using App.Core.Model;
using Core.Interface;
using Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class GetAllProductQuery :IRequest<ResponceDto>
    {
        public int Id { get; set; }
    }
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllProductQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {

            var user=await _appDbContext.Set<User>().FirstOrDefaultAsync((x)=> x.Id == request.Id);
            if (user == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong"
                };
            }

            if(user.UserType_Id==1)
            {
                var allProduct = await _appDbContext.Set<Product>()
              .Where((x) => x.IsDeleted == 0 && x.Stock > 0)
              .ToListAsync();

                if (allProduct != null && allProduct.Any())
                {
                    return new ResponceDto
                    {
                        statusCode = 200,
                        isSuccess = true,
                        data = allProduct
                    };
                }
                else
                {
                    return new ResponceDto
                    {
                        statusCode = 401,
                        isSuccess = false,
                        message = "no products available Please add some products"
                    };
                }

            }
            if(user.UserType_Id==2)
            {
                var allProduct = await _appDbContext.Set<Product>()
              .Where((x) => x.IsDeleted == 0 && x.Stock > 0 && x.IsCreatedCy==request.Id)
              .ToListAsync();

                if (allProduct != null && allProduct.Any())
                {
                    return new ResponceDto
                    {
                        statusCode = 200,
                        isSuccess = true,
                        data = allProduct
                    };
                }
                else
                {
                    return new ResponceDto
                    {
                        statusCode = 401,
                        isSuccess = false,
                        message = "no products available Please add some products"
                    };
                }

            }

            return new ResponceDto
            {
                isSuccess = false,
                message = "something went wrong"
            };


        }
    }
}
