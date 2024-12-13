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
    public class UpdateProductCommand:IRequest<ResponceDto>
    {
        public ProductDto Product { get; set; }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public UpdateProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var oldProduct = await _appDbContext.Set<Product>()
                .FirstOrDefaultAsync((x) => x.Id == request.Product.ProductId);

            if (oldProduct != null)
            {

                oldProduct.ProductName = request.Product.ProductName;
                oldProduct.ProductImage = request.Product.ProductImage;
                oldProduct.Category = request.Product.Category;
                oldProduct.Brand = request.Product.Brand;
                oldProduct.SellingPrice = request.Product.SellingPrice;
                oldProduct.PurchasePrice = request.Product.PurchasePrice;
                oldProduct.PurchaseDate = request.Product.PurchaseDate;
                oldProduct.Stock = request.Product.Stock;

                await _appDbContext.SaveChangesAsync();
                return new ResponceDto
                {
                    isSuccess = true,
                    message = "product updated successfully"
                };

            }

            else
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "an error occured please try again later"
                };
           
        }
    }
}
