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
    public class LoginQuery:IRequest<ResponceDto>
    {
        public LoginDto Login { get; set; }
    }
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public LoginQueryHandler(IAppDbContext appDbContext,IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }
        public async Task<ResponceDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var loginDetails = request.Login;

            var isExist = await _appDbContext.Set<UserDetails>()
                .FirstOrDefaultAsync((x) => x.UserName == loginDetails.UserName);

            if (isExist == null) {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "invalid userName "
                };
            }
            else
            {
                if(!BCrypt.Net.BCrypt.Verify(loginDetails.Password,isExist.Password))
                {
                    return new ResponceDto
                    {
                        isSuccess = false,
                        message = "invalid password "
                    };
                }

                var otpVal = new Random().Next(100000, 999999).ToString();
                Otp newOtpObj = new Otp
                {
                    UserName = isExist.UserName,
                    otp = otpVal
                };

                var isUserOtpExist = await _appDbContext.Set<Otp>()
                 .FirstOrDefaultAsync((x) => x.UserName == isExist.UserName);

                if (isUserOtpExist == null)
                { 
                await _appDbContext.Set<Otp>().AddAsync(newOtpObj);
                }
                else
                {
                    isUserOtpExist.otp = otpVal;
                }
                await _appDbContext.SaveChangesAsync();

                var isSend = await _emailService.SendEmailAsync("Otp Authentication"
                   , isExist.Email
                   , $"Your otp is {otpVal}");



                return new ResponceDto
                {
                    statusCode = 200,
                    message = "Otp sended to your regestered Email ID",
                    isSuccess = true,
                 };

            }


        }
    }

}
