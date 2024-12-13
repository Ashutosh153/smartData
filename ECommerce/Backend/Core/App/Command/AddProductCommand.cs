using App.Core.Model;
using Core.Interface;
using Domain.User;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Command
{
    public class AddProductCommand:IRequest<ResponceDto>
    {
        public ProductDto NewProduct { get; set; }
    }
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public AddProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {

            //need to update the string of the image before adding to database

            if(request.NewProduct.PurchasePrice>request.NewProduct.SellingPrice)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "selling price must be grater than purchase price"
                };
            }
           
            var finalProduct = request.NewProduct.Adapt<Product>();
            var Maxid = await _appDbContext.Set<Product>()
                         .Select(e => e.Id)
                         .DefaultIfEmpty()
                         .MaxAsync();
            finalProduct.ProductCode="PR"+(Maxid+1).ToString().PadLeft(4,'0');

            //finalProduct.ProductImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQOtjqFKVwZWNCqI33H1OWcsUaZYww6FLLFAw&s";

            await _appDbContext.Set<Product>().AddAsync(finalProduct);
            await _appDbContext.SaveChangesAsync();


            return new ResponceDto
            {
                statusCode = 200,
                isSuccess = true,
                message ="Product Added successfully"
            };







           
        }
    }
}
