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
    public class VerifyAvailableAppointmentQuerry:IRequest<ResponceDto>
    {
        public AddAppointmentDto verifyAppointment { get; set; }
    }
    public class VerifyAvailableAppointmentQuerryHandler : IRequestHandler<VerifyAvailableAppointmentQuerry, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public VerifyAvailableAppointmentQuerryHandler(IAppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }
        public async Task<ResponceDto> Handle(VerifyAvailableAppointmentQuerry request, CancellationToken cancellationToken)
        {

            var GetAppointmentDetails = await _appDbContext.Set<Appointments>()
                .FirstOrDefaultAsync((x) => x.ProviderId == request.verifyAppointment.ProviderId
                && x.AppointmentDate == request.verifyAppointment.AppointmentDate
                && x.AppointmentTime == request.verifyAppointment.AppointmentTime
                &&x.AppointmentStatus== "Scheduled");

            if (GetAppointmentDetails != null) 
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "there is no appointment available at that time \n please select another time for the appointment"

                };
            }

            else
            {
                return new ResponceDto
                {
                    isSuccess = true,
                    message="you can book the appointment"
                };
            }

            
        }
    }
}
