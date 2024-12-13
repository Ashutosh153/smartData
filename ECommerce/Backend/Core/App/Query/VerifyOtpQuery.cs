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
    public class VerifyOtpQuery:IRequest<ResponceDto>
    {
        public Otp otpDetails { get; set; } 
    }
    public class VerifyOtpQueryHandler : IRequestHandler<VerifyOtpQuery, ResponceDto>
    {
        private readonly IConfiguration _configuration;
        private readonly IAppDbContext _appDbContext;

        public VerifyOtpQueryHandler( IAppDbContext appDbContext,IConfiguration configuration)
        {
            _configuration = configuration;
            _appDbContext = appDbContext;
        }


        public async Task<ResponceDto> Handle(VerifyOtpQuery request, CancellationToken cancellationToken)
        {

            var isExist=await _appDbContext.Set<User>()
                .FirstOrDefaultAsync((x)=>x.Username==request.otpDetails.UserName);

            if (isExist == null) {
                return new ResponceDto
                {
                    isSuccess = false,
                    message="invalid credentials"

                }; 
            }

            var vlaidateOtp = await _appDbContext.Set<Otp>()
                            .FirstOrDefaultAsync((x) => x.UserName == request.otpDetails.UserName 
                            && x.otp==request.otpDetails.otp);

            if (vlaidateOtp == null) {
                return new ResponceDto
                {
                    isSuccess=false,
                    message="invalid otp"
                };
            }
            var clamis = new[]
                      {
                                 new Claim(JwtRegisteredClaimNames.Sub,_configuration["jwt:subject"]),
                                 new Claim("id",isExist.Id.ToString()),
                                 new Claim("email",isExist.Email),
                                 new Claim(ClaimTypes.Role,isExist.UserType_Id.ToString())
                          };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var readToken = new JwtSecurityToken(
                _configuration["jwt:Issuers"],
                _configuration["jwt:Audiences"],
                clamis,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signin
                );
            var writeToken = new JwtSecurityTokenHandler().WriteToken(readToken);


            

            return new ResponceDto
            {
                statusCode = 200,
                isSuccess = true,
                message = "Login successfull",
                data = new {
                    Token=writeToken,
                    id=isExist.UserType_Id
                    
                }
            };

            
        }
    }
}
