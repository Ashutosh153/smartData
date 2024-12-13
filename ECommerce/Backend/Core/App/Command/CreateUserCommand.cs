using App.core.interfaces;
using App.Core.Model;
using Core.Interface;
using Domain.User;
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
        public RegestrationDto newUser {  get; set; } 
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponceDto>
    {
        public readonly IAppDbContext _appDbContext;
        public readonly IEmailService _emailService;
        public CreateUserCommandHandler(IAppDbContext appDbContext, IEmailService emailService) 
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }
        public async Task<ResponceDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            RegestrationDto NewUser = request.newUser;
            var isExist = await _appDbContext.Set<User>().FirstOrDefaultAsync((x) => x.Email == NewUser.Email);
            if (isExist != null)
            {
                return new ResponceDto
                {
                    message = "email already exist",
                    isSuccess = false
                };
            }

            else
            {
                string userName = ("EC_"
                                    + NewUser.LastName
                                    + NewUser.FirstName.ToCharArray()[0]
                                    + NewUser.DOB.ToString("ddMMyy"))
                                    .ToUpper();

                              Random rnd = new Random();
              
                              const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
                              var lengthOfString = chars.Length;

                var pass=new string(Enumerable.Repeat(chars,8)
                              .Select(s => s[rnd.Next(lengthOfString)]).ToArray());



           

            var userDetails = NewUser.Adapt<User>();

                userDetails.Username = userName;
                userDetails.Password= BCrypt.Net.BCrypt.HashPassword(pass); 


                await _appDbContext.Set<User>().AddAsync(userDetails);



                var isSend= await _emailService.SendEmailAsync
                    ($"Welcome {userDetails.FirstName} {userDetails.LastName} "
                    , userDetails.Email
                    , $"your User Name is : {userDetails.Username},\n Your Password is : {pass}");


                if(isSend)
                {
                    await _appDbContext.SaveChangesAsync();
                }
             



                return new ResponceDto
                {
                    statusCode = 200,
                    isSuccess = true,
                    message = "regestration is successfull \n please check your email user Name and Password"
                   
                }; 


            }

            //throw new NotImplementedException();
        }
    }
}
