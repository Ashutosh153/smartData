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
    public class UpdateProviderCommand : IRequest<ResponceDto>
    {
        public UpdateProviderDto providerDto {  get; set; }
    }
    public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public UpdateProviderCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
        {
            var newProviderDetails = request.providerDto;

            var oldProviderDetails=await _appDbContext.Set<UserDetails>()
                .FirstOrDefaultAsync((x)=>x.Id== newProviderDetails.Id);



            if (oldProviderDetails == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message="something went wrong please try again later"
                };
            }
            else
            {


                oldProviderDetails.FirstName = newProviderDetails.FirstName;
                oldProviderDetails.LastName = newProviderDetails.LastName;
                oldProviderDetails.DateOfBirth = newProviderDetails.DateOfBirth;
                oldProviderDetails.Gender = newProviderDetails.Gender;
                oldProviderDetails.BloodGroup = newProviderDetails.BloodGroup;
                oldProviderDetails.Address = newProviderDetails.Address;
                oldProviderDetails.CityId = newProviderDetails.CityId;
                oldProviderDetails.StateId = newProviderDetails.StateId;
                oldProviderDetails.Mobile = newProviderDetails.Mobile;
                //oldProviderDetails.CountryId= newProviderDetails.CountryId;
                oldProviderDetails.PinCode= newProviderDetails.PinCode;
                oldProviderDetails.Profile = newProviderDetails.Profile;

                 oldProviderDetails.Qualification= newProviderDetails.Qualification;
                oldProviderDetails.Specialisation_ID = newProviderDetails.Specialisation_ID;
                oldProviderDetails.Registration_Number = newProviderDetails.Registration_Number;
                         oldProviderDetails.Visiting_Charge= newProviderDetails.Visiting_Charge;

                await _appDbContext.SaveChangesAsync();

                return new ResponceDto
                {
                    isSuccess = true,
                    message = "Profile updated successfully"
                };


                
            }
        }
    }

}
