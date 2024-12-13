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
    public class UpdateUserCommand:IRequest<ResponceDto>
    {
        public UpdateUserDto UpdateUserValue { get; set; }
    }

    public class UpdateUserCommandHAndler : IRequestHandler<UpdateUserCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public UpdateUserCommandHAndler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser=await _appDbContext.Set<User>()
                .FirstOrDefaultAsync((x)=>x.Id==request.UpdateUserValue.Id);

            if (existingUser == null)
            {
                return new ResponceDto
                {
                    statusCode = 401,
                    isSuccess = false,
                    message = "user not found"

                };
             }

            existingUser.FirstName = request.UpdateUserValue.FirstName;
            existingUser.LastName = request.UpdateUserValue.LastName;   
           // existingUser.Username=request.UpdateUserValue.userName;
            //existingUser.Email=request.UpdateUserValue.Email;
            existingUser.Mobile=request.UpdateUserValue.Mobile;
            existingUser.DOB = request.UpdateUserValue.DOB;
            existingUser.Address = request.UpdateUserValue.Address;
            existingUser.Zipcode = request.UpdateUserValue.Zipcode;
            existingUser.Country_Id = request.UpdateUserValue.Country_Id;
            existingUser.State_Id = request.UpdateUserValue.State_Id;
            existingUser.ProfileImage = request.UpdateUserValue.Profile;




            await _appDbContext.SaveChangesAsync();

            return new ResponceDto
            {
                statusCode = 200,
                isSuccess = true,
                message = "user Updated successfull"

            };

        }
    }
}
