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
    public class RemoveItemFromCartCommand:IRequest<ResponceDto>
    {
        public RemoveFromCartDto cartDto { get; set; }  
    }

    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public RemoveItemFromCartCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var cartId= await _appDbContext.Set<CartMaster>()
                .FirstOrDefaultAsync((x)=>x.UserId==request.cartDto.userId);

            if (cartId == null) {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong please try again later"
                };
            }

            var isExist = await _appDbContext.Set<CartDetail>()
                .FirstOrDefaultAsync((x) => x.CartId == cartId.Id && x.ProductId == request.cartDto.productId);

            var product = await _appDbContext.Set<Product>()
                .FirstOrDefaultAsync((x)=>x.Id==request.cartDto.productId);  

            if (isExist == null) {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong please try again later"
                };
            }

            

             _appDbContext.Set<CartDetail>()
                .Remove(isExist);

            product.Stock += isExist.Qty;

            await _appDbContext.SaveChangesAsync();

            return new ResponceDto
            {
                isSuccess = true,
                message = "product removed successfully"
            };
        }
    }
}
