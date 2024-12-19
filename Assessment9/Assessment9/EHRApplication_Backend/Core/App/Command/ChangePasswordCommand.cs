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

namespace App.Core.App.Command
{
    public class ChangePasswordCommand : IRequest<ResponceDto>
    {
        public ChangePasswordDto chnagePassObj { get; set; }
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;

        public ChangePasswordCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == request.chnagePassObj.Id);

            if (user == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong"
                };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.chnagePassObj.Password);
            await _appDbContext.SaveChangesAsync();
            return new ResponceDto
            {
                isSuccess = true,
                message = "password changed successfully"
            };
        }
    }
}
