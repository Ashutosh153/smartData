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

namespace App.Core.App.Command
{
    public class DecrementQuantitiCommand : IRequest<ResponceDto>
    {
        public IncDecObjDto IncDecObj { get; set; }
    }

    public class DecrementQuantitiCommandHandler : IRequestHandler<DecrementQuantitiCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public DecrementQuantitiCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(DecrementQuantitiCommand request, CancellationToken cancellationToken)
        {

            var productInCart=await _appDbContext.Set<CartDetail>()
                .FirstOrDefaultAsync((x)=>x.CartId==request.IncDecObj.cartId 
                                        && x.ProductId==request.IncDecObj.productId);  

            if (productInCart == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong please try again later"
                };
            }

            else
            {
                if(productInCart.Qty==1)
                {
                     _appDbContext.Set<CartDetail>().Remove(productInCart);

                }

                else
                {
                    productInCart.Qty -= 1;

                }
                
                var stockInDb=await _appDbContext.Set<Product>()
                    .FirstOrDefaultAsync((x)=>x.Id==request.IncDecObj.productId);

                stockInDb.Stock += 1;

                await _appDbContext.SaveChangesAsync();

                return new ResponceDto
                {
                    isSuccess = true,
                    message="decrement successfull "
                };
            }

           

            
        }
    }
}