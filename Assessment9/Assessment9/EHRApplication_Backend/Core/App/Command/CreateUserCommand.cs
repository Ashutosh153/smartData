using App.Core.ModelsDto;
using Core.Interface;
using Domain.Models.User;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Command
{
    public class CreateUserCommand:IRequest<ResponceDto>
    {
        public RegModelDto newUser {  get; set; }   
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public CreateUserCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }
        public async Task<ResponceDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var isExist = await _appDbContext.Set<UserDetails>()
                .FirstOrDefaultAsync((x) => x.Email == request.newUser.Email);
            if (isExist != null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "email already Exist"
                };

            }
            else
            {
                string userNameVal = null;
                if (request.newUser.UserType_ID == 1) 
                {
                     userNameVal = ("PT_"
                                  + request.newUser.FirstName
                                  + request.newUser.LastName.ToCharArray()[0]
                                  + request.newUser.DateOfBirth.ToString("ddMMyy"))
                                  .ToUpper();
                }
                if(request.newUser.UserType_ID==2)
                {
                     userNameVal = ("PR_"
                                 + request.newUser.FirstName
                                 + request.newUser.LastName.ToCharArray()[0]
                                 + request.newUser.DateOfBirth.ToString("ddMMyy"))
                                 .ToUpper();

                    if(request.newUser.Qualification==null|| request.newUser.Specialisation_ID==null||
                        request.newUser.Registration_Number == null|| request.newUser.Visiting_Charge==null)
                    {
                        return new ResponceDto
                        {
                            isSuccess = false,
                            message = "please enter all the data properly"
                        };
                    }
                }

                Random rnd = new Random();

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

                var pass = new string(Enumerable.Repeat(chars, 8)
                              .Select(s => s[rnd.Next(chars.Length)]).ToArray());


                var newUserObj = request.newUser.Adapt<UserDetails>();

                newUserObj.UserName = userNameVal;
                newUserObj.Password = BCrypt.Net.BCrypt.HashPassword(pass);

                await _appDbContext.Set<UserDetails>().AddAsync(newUserObj);

                var isSend = await _emailService.SendEmailAsync
                    ($"Welcome {newUserObj.FirstName} {newUserObj.LastName} "
                    , newUserObj.Email
                    , $"your User Name is : {newUserObj.UserName},\n Your Password is : {pass}");

                await _appDbContext.SaveChangesAsync();

                return new ResponceDto
                {
                    isSuccess = true,
                    message = "regestration successfull \n please check your email for user name and password"

                };

            }


           // throw new NotImplementedException();
        }
    }
}
