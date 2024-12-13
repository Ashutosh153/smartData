 using App.core.interfaces;
using App.Core.Model;
using Core.Interface;
using Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class LoginQuery : IRequest<ResponceDto>
    {
        public LoginDto _loginDetails { get; set; }
    }
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public LoginQueryHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }
        public async Task<ResponceDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var loginDetails = request._loginDetails;

            var isExist = await _appDbContext.Set<User>()
                .FirstOrDefaultAsync((x) => x.Username == loginDetails.userName);

            if (isExist == null)
            {
                return new ResponceDto
                {
                    message = "invalid user name ",
                    isSuccess = false
                };
            }
            else
            {
                if(!BCrypt.Net.BCrypt.Verify(loginDetails.password,isExist.Password))
                {
                    return new ResponceDto
                    {
                        statusCode=401,
                        message = "invalid Password",
                        isSuccess = false
                    };
                }


            


                var otpVal = new Random().Next(100000, 999999).ToString();
                Otp newOtpObj = new Otp {
                    UserName = isExist.Username,
                    otp = otpVal
                };

                var isUserOtpExist=await _appDbContext.Set<Otp>()
                    .FirstOrDefaultAsync((x)=>x.UserName == isExist.Username);

                if (isUserOtpExist == null)
                {
                    await _appDbContext.Set<Otp>().AddAsync(newOtpObj);

                }
                else { 
                    
                    isUserOtpExist.otp=otpVal;
                }

                await _appDbContext.SaveChangesAsync();

           var isSend=     await _emailService.SendEmailAsync("Otp Authentication"
                    , isExist.Email
                    , $"Your otp is {otpVal}");



                return new ResponceDto
                {
                    statusCode = 200,   
                    message="Otp sended to your regestered Email ID",
                    isSuccess = true,
                   

                };


            }
        }
    }
}
