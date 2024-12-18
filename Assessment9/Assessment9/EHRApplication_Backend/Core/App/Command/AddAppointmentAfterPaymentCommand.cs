using App.Core.Interface;
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
    public class AddAppointmentAfterPaymentCommand:IRequest<ResponceDto>
    {
        public AddAppointmentByPaymentDto AddAppointmentDto { get; set; }
    }
    public class AddAppointmentAfterPaymentCommandHandler : IRequestHandler<AddAppointmentAfterPaymentCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;   
        private readonly IRazorpayService _razorpayService;
        private readonly IEmailService _emailService;
        public AddAppointmentAfterPaymentCommandHandler(IAppDbContext appDbContext,IRazorpayService razorpayService, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _razorpayService = razorpayService;
            _emailService = emailService;
        }
        public async Task<ResponceDto> Handle(AddAppointmentAfterPaymentCommand request, CancellationToken cancellationToken)
        {
            var patient = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == request.AddAppointmentDto.PatientId);
            var practioner = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == request.AddAppointmentDto.ProviderId);

            var paymentData = await _razorpayService.VerifyPaymentAsync(request.AddAppointmentDto.PaymentId, request.AddAppointmentDto.OrderId);

            var newAppointment = request.AddAppointmentDto.Adapt<Appointments>();
            newAppointment.AppointmentStatus = "Scheduled";
            await _appDbContext.Set<Appointments>().AddAsync(newAppointment);
            await _appDbContext.SaveChangesAsync();

            await _emailService.SendEmailAsync("New Appointmwnt Scheduled", practioner.Email,
              $"new appointment has been scheduled on {request.AddAppointmentDto.AppointmentDate}  at{request.AddAppointmentDto.AppointmentTime} \n please be available at that moment");

            await _emailService.SendEmailAsync("Appointmwnt Scheduled", patient.Email,
                           $"the appointment has been scheduled on {request.AddAppointmentDto.AppointmentDate}  at{request.AddAppointmentDto.AppointmentTime} \n successfully");

            return new ResponceDto
            {
                message = "appointment created successfully",
                isSuccess = true,
                data = paymentData
            };

            // throw new NotImplementedException();
        }
    }
}
