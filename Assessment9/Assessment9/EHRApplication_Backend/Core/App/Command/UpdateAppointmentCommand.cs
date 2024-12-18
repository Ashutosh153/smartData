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
        private readonly IEmailService _emailService;
        public UpdaetAppointmentCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
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


                //if(OldAppointment.AppointmentDate!=request.updateappointment.AppointmentDate||
                //    OldAppointment.AppointmentTime!=request.updateappointment.AppointmentTime||
                //    OldAppointment.ChiefComplaint!=request.updateappointment.ChiefComplaint)
                {

                    var patient = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == OldAppointment.PatientId);
                    var practioner = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == OldAppointment.ProviderId);
                    await _emailService.SendEmailAsync("Update in Appointmwnt Scheduled", practioner.Email,
                        $"the appointment with appointment id {OldAppointment.Id} has been updated and rescheduled on {request.updateappointment.AppointmentDate}  at {request.updateappointment.AppointmentTime} \n please be available at that moment");

                    await _emailService.SendEmailAsync("Update Appointment Scheduled", patient.Email,
                                   $"the appointment id  appointment id {OldAppointment.Id} has been updated  scheduled on {request.updateappointment.AppointmentDate}  at{request.updateappointment.AppointmentTime} \n successfully");




                }




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
