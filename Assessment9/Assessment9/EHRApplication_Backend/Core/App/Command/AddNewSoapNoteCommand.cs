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
    public class AddNewSoapNoteCommand:IRequest<ResponceDto>
    {
        public SOAPNotesDetails newSoapNote {  get; set; }
    }
    public class AddNewSoapNoteCommandHandler : IRequestHandler<AddNewSoapNoteCommand, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public AddNewSoapNoteCommandHandler(IAppDbContext appDbContext,IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        public async Task<ResponceDto> Handle(AddNewSoapNoteCommand request, CancellationToken cancellationToken)
        {
            var isExist=await _appDbContext.Set<SOAPNotesDetails>().FirstOrDefaultAsync((x)=>x.AppointmnetID==request.newSoapNote.AppointmnetID);

            if (isExist == null)
            {

                await _appDbContext.Set<SOAPNotesDetails>().AddAsync(request.newSoapNote);

                var appointmentStatus = await _appDbContext.Set<Appointments>()
                    .FirstOrDefaultAsync((x) => x.Id == request.newSoapNote.AppointmnetID && x.AppointmentStatus== "Scheduled");

                if (appointmentStatus != null) 
                {
                    appointmentStatus.AppointmentStatus = "Completed";
                    await _appDbContext.SaveChangesAsync();

                 var appdetails = await _appDbContext.Set<Appointments>()
                    .FirstOrDefaultAsync((x) => x.Id == request.newSoapNote.AppointmnetID);
                    if(appdetails !=null)
                    {

                        var practobj = await _appDbContext.Set<UserDetails>()
                            .FirstOrDefaultAsync((x) => x.Id == appdetails.ProviderId);

                        var patientObj = await _appDbContext.Set<UserDetails>()
                            .FirstOrDefaultAsync((x) => x.Id == appdetails.PatientId);
                        await _emailService.SendEmailAsync(" Appointment Completed ", practobj.Email,
                     $"the appointment with appointmentID {appointmentStatus.Id} has been completed successfully  ");

                        await _emailService.SendEmailAsync("Appointmwnt Completed", patientObj.Email,
                                       $"the appointment with appointmentID {appointmentStatus.Id}been successfully Completed " +
                                       $" with details :<br>" +
                                       $"<b>Subjective</b> : {request.newSoapNote.Subjective}<br>" +
                                       $"<b>Objective</b> : {request.newSoapNote.Objective}<br>" +
                                       $"<b>Assessment</b>: {request.newSoapNote.Assessment}<br>" +
                                       $"<b>Plan</b> : {request.newSoapNote.Plan }");

                    }


                    return new ResponceDto
                    {
                        isSuccess = true,
                        message = "appointment completed successfully"
                    };
                }
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "appointment closed already"
                };

            }
            return new ResponceDto
            {
                isSuccess = false,
                message = "something went wrong please try again later"
            };

        }
    }
}
