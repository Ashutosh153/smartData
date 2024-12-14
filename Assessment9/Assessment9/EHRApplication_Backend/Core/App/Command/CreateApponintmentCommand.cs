using App.Core.ModelsDto;
using Core.Interface;
using Domain.Models.User;
using Mapster;
using MediatR;
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
        public CreateApponintmentCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(CreateApponintmentCommand request, CancellationToken cancellationToken)
        {



            var app = request.AddAppointmentDto.Adapt<Appointments>();
            app.AppointmentStatus = "Scheduled";
             await _appDbContext.Set<Appointments>().AddAsync(app);
            
            await _appDbContext.SaveChangesAsync();

            return new ResponceDto
            {
                isSuccess = true,
                
            };

            //throw new NotImplementedException();
        }
    }
}
