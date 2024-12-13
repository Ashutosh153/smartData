using App.Core.ModelsDto;
using Core.Interface;
using Domain.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class GetAllUsersQuery:IRequest<ResponceDto>
    {
        public int userId {  get; set; }
    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ResponceDto>
    {
        IAppDbContext _appDbContext;
        public GetAllUsersQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var isExist=await _appDbContext.Set<UserDetails>()
                .FirstOrDefaultAsync((x)=>x.Id==request.userId);

            if (isExist==null)
            {
                return new ResponceDto
                {
                    
                    isSuccess = false,
                    message = "something went wrong please try again later",
                    
                };
            }

            var allUsers = await _appDbContext.Set<UserDetails>().
                Where((x) => x.UserType_ID != isExist.UserType_ID).ToListAsync();

            return new ResponceDto
            {

                isSuccess = true,
                data= allUsers

            };


           // throw new NotImplementedException();
        }
    }
}
