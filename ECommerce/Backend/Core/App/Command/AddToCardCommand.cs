using App.Core.Model;
using Core.Interface;
using Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Command
{
    public class AddToCartCommand:IRequest<ResponceDto>
    {
        public AddToCartDto AddToCard { get; set; }
    }

    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public AddToCartCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
          
            AddToCartDto cartVal= request.AddToCard;
            
            var isExist=await _appDbContext.Set<CartMaster>()
                .FirstOrDefaultAsync((x)=>x.UserId==request.AddToCard.userId);
            if (isExist == null) {

         

                isExist = new CartMaster
                {
                    UserId = request.AddToCard.userId,
                };
                await _appDbContext.Set<CartMaster>().AddAsync(isExist);
                    await _appDbContext.SaveChangesAsync();
            }


            var AlreadyAdded = await _appDbContext.Set<CartDetail>()
                .FirstOrDefaultAsync((x) => x.CartId == isExist.Id && x.ProductId==cartVal.productId);
            var product = await _appDbContext.Set<Product>()
                .FirstOrDefaultAsync((x)=>x.Id==cartVal.productId && x.Stock>0);

            if (AlreadyAdded == null && product !=null) {
                await _appDbContext.Set<CartDetail>().AddAsync( new CartDetail
                {
                    CartId=isExist.Id,
                    ProductId=cartVal.productId,
                    Qty=cartVal.quantity
                });

                product.Stock -= 1;
                await _appDbContext.SaveChangesAsync();


            }



            return new ResponceDto
            {
                isSuccess = true,
                message = "product addded successfully"
            };


        }
    }
}
