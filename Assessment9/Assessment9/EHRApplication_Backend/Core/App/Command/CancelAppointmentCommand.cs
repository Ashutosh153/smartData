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
    public class CancelAppointmentCommand : IRequest<ResponceDto>
    {
        public int AppointmentId { get; set; }
    }
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public CancelAppointmentCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {

            _appDbContext = appDbContext;
            _emailService = emailService;
        }
        public async Task<ResponceDto> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var completedAppointment = await _appDbContext.Set<Appointments>().FirstOrDefaultAsync((x) => x.Id == request.AppointmentId);
            if (completedAppointment != null)
            {
                completedAppointment.AppointmentStatus = "Canceled";
                await _appDbContext.SaveChangesAsync();



                var patient = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == completedAppointment.PatientId);
                var practioner = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == completedAppointment.ProviderId);

                await _emailService.SendEmailAsync(" Appointmwnt Cancelletion", practioner.Email,  
                    $"the appointment with appointmentID {completedAppointment.Id}been cancelled \n  sorry for inconvinence");

                await _emailService.SendEmailAsync("Appointmwnt Cancelletion", patient.Email,
                               $"the appointment with appointmentID {completedAppointment.Id}been successfully");








                return new ResponceDto
                {
                    isSuccess = true,
                    message = "appointment Canceled successfully"
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
