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
    public class IncrementQuantitiCommand:IRequest<ResponceDto>
    {
        public IncDecObjDto IncDecVal { get; set; }
    }

    public class IncrementQuantitiCommandHandler : IRequestHandler<IncrementQuantitiCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public IncrementQuantitiCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(IncrementQuantitiCommand request, CancellationToken cancellationToken)
        {

          var cartProduct= await _appDbContext.Set<CartDetail>()
                      .FirstOrDefaultAsync((x) => x.CartId == request.IncDecVal.cartId && x.ProductId == request.IncDecVal.productId);

            if(cartProduct!=null)
            {
                var QuentityInDb = await _appDbContext.Set<Product>().
                    FirstOrDefaultAsync((x) => x.Id == request.IncDecVal.productId && x.IsDeleted == 0);

                if (QuentityInDb != null)
                {
                    if (QuentityInDb.Stock >= 1)
                    {

                        cartProduct.Qty +=1;
                        QuentityInDb.Stock -= 1;

                        await _appDbContext.SaveChangesAsync();

                        return new ResponceDto
                        {
                            isSuccess = true,
                            message = "product increment successfully"
                        };
                    }
                    else
                    {
                        return new ResponceDto
                        {
                            isSuccess = false,
                            message = "Product out of stock"
                        };
                    }

                    
                }

                else
                {
                    _appDbContext.Set<CartDetail>().Remove(cartProduct);
                    await _appDbContext.SaveChangesAsync();

                    return new ResponceDto
                    {
                        isSuccess = false,
                        message = "sorry for inconvinence"
                    };
                }
               



            }
            return new ResponceDto
            {
                isSuccess = false,
                message = "something went wrong please try again later"
            };



           
        }
    }

}
