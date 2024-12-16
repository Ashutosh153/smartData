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
    public class UpdateAppointmentCommand:IRequest<ResponceDto>
    {
        public UpdateAppointmentDto updateappointment {  get; set; }
    }
    public class UpdaetAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public UpdaetAppointmentCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var OldAppointment= await _appDbContext.Set<Appointments>()
                .FirstOrDefaultAsync((x)=>x.Id==request.updateappointment.id && x.AppointmentStatus == "Scheduled");

            if (OldAppointment != null)
            {
                OldAppointment.AppointmentDate = request.updateappointment.AppointmentDate;
                OldAppointment.AppointmentTime = request.updateappointment.AppointmentTime;
                OldAppointment.ChiefComplaint = request.updateappointment.ChiefComplaint;

                await _appDbContext.SaveChangesAsync();

                return new ResponceDto
                {
                    isSuccess = true,
                    message = "appointment updated successfully "
                };
            }


            else
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong please try again later"
                };
            }
        }
    }
}
