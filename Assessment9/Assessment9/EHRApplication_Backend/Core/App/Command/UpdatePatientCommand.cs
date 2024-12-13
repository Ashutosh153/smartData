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
    public class UpdatePatientCommand:IRequest<ResponceDto>
    {
        public UpdatePatientDto UpdatePatientDto { get; set; }
    }

    public class UpdatePatientCommandHAndler : IRequestHandler<UpdatePatientCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public UpdatePatientCommandHAndler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            UpdatePatientDto NewPatientDetails = request.UpdatePatientDto;

            var oldPatientDetails = await _appDbContext.Set<UserDetails>()
                .FirstOrDefaultAsync((x) => x.Id ==NewPatientDetails.Id);

            if (oldPatientDetails == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong please try again later"
                };
            }
            else
            {
                oldPatientDetails.FirstName=NewPatientDetails.FirstName;
                oldPatientDetails.LastName=NewPatientDetails.LastName;
                oldPatientDetails.DateOfBirth=NewPatientDetails.DateOfBirth;
                oldPatientDetails.Gender=NewPatientDetails.Gender;
                oldPatientDetails.BloodGroup=NewPatientDetails.BloodGroup;
                oldPatientDetails.Address=NewPatientDetails.Address;
                oldPatientDetails.StateId=NewPatientDetails.StateId;
                oldPatientDetails.CityId=NewPatientDetails.CityId;
                oldPatientDetails.Mobile = NewPatientDetails.Mobile;
               // oldPatientDetails.CountryId=NewPatientDetails.CountryId;
                oldPatientDetails.PinCode=NewPatientDetails.PinCode;
                oldPatientDetails.Profile = NewPatientDetails.Profile;

                await _appDbContext.SaveChangesAsync();

                return new ResponceDto
                {
                    isSuccess = false,
                    message = "patient updated successfully"
                };
            }


        }
    }
}
