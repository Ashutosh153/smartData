using App.Core.ModelsDto;
using Core.Interface;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Command
{
    public class CompleteAppointmentCommand:IRequest<ResponceDto>
    {
        public int AppointmentId {  get; set; } 
    }
    public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public CompleteAppointmentCommandHandler(IAppDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var completedAppointment=await _appDbContext.Set<Appointments>().FirstOrDefaultAsync((x)=>x.Id==request.AppointmentId);
            if (completedAppointment != null)
            {
                completedAppointment.AppointmentStatus = "Completed";
                await _appDbContext.SaveChangesAsync();
                return new ResponceDto
                {
                    isSuccess = true,
                    message = "appointment completd successfully"
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
