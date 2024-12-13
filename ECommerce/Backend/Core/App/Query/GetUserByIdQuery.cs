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
    public class GetUserByIdQuery:IRequest<ResponceDto>
    {
        public int Id { get; set; }
    }
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public GetUserByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user=await _appDbContext.Set<User>()
                .FirstOrDefaultAsync((x)=>x.Id == request.Id);

            if (user == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message="user not found"
                };
            }

            return new ResponceDto
            {
                isSuccess = true,
                data = user,
                message = "user fetched successfully"
            };

            
        }
    }
}
