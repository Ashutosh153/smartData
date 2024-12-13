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
    public class DeleteProductCommand:IRequest<ResponceDto>
    {
        public int id {  get; set; }    
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
         {

            var isExist=await _appDbContext.Set<Product>().FirstOrDefaultAsync((x)=>x.Id==request.id);
            if (isExist==null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "an error occured please try again later"
                };
            }
            isExist.IsDeleted = 1;
          await  _appDbContext.SaveChangesAsync();

            return new ResponceDto
            {
                isSuccess = true,
                message = "product deleted successfully"
            };
        }
    }
}
