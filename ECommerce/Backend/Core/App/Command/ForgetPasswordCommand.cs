using App.core.interfaces;
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
    public class ForgetPasswordCommand:IRequest<ResponceDto>
    {
        public string Email {  get; set; }
    }

    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public ForgetPasswordCommandHandler(IAppDbContext appDbContext,IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }
        public async Task<ResponceDto> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {

            var isExist=await _appDbContext.Set<User>()
                .FirstOrDefaultAsync((x)=>x.Email == request.Email);

            if (isExist == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message="invalid email",
                    statusCode=401
                };
            }

            Random rnd = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var pass = new string(Enumerable.Repeat(chars, 8)
                          .Select(s => s[rnd.Next(chars.Length)]).ToArray());

            isExist.Password = BCrypt.Net.BCrypt.HashPassword(pass);
            await _appDbContext.SaveChangesAsync();

        var isSend=  await  _emailService.SendEmailAsync("password recovery", isExist.Email, $"your new Password is {pass}");


            //if (isSend == false) {

            //    return new ResponceDto
            //    {
            //        isSuccess = false,
            //        statusCode=400
                   
            //    };
            //}

            return new ResponceDto
            {
                isSuccess = true,
                statusCode = 200,
                message = "new password is sended to your registured Email"
            };
            
        }
    }
}
