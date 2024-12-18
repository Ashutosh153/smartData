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
    public class CreateApponintmentCommand:IRequest<ResponceDto>
    {
        public AddAppointmentDto AddAppointmentDto { get; set; }
    }
    public class CreateApponintmentCommandHandler : IRequestHandler<CreateApponintmentCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public CreateApponintmentCommandHandler(IAppDbContext appDbContext,IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService=emailService;
        }

        public async Task<ResponceDto> Handle(CreateApponintmentCommand request, CancellationToken cancellationToken)
        {

            var patient= await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x)=>x.Id==request.AddAppointmentDto.PatientId);
            var practioner= await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x)=>x.Id==request.AddAppointmentDto.ProviderId);

            var app = request.AddAppointmentDto.Adapt<Appointments>();
            app.AppointmentStatus = "Scheduled";
             await _appDbContext.Set<Appointments>().AddAsync(app);
            
            await _appDbContext.SaveChangesAsync();

            await _emailService.SendEmailAsync("New Appointmwnt Scheduled", practioner.Email,
                $"new appointment has been scheduled on {request.AddAppointmentDto.AppointmentDate}  at{request.AddAppointmentDto.AppointmentTime} \n please be available at that moment");

            await _emailService.SendEmailAsync("Appointmwnt Scheduled", patient.Email,
                           $"the appointment has been scheduled on {request.AddAppointmentDto.AppointmentDate}  at{request.AddAppointmentDto.AppointmentTime} \n successfully");




            return new ResponceDto
            {
                message="appointment created successfully",
                isSuccess = true,
                
            };

            //throw new NotImplementedException();
        }
    }
}
