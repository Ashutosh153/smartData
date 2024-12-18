using App.Core.ModelsDto;
using Core.Interface;
using Dapper;
using Domain.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class GetAllAppointmentsQuerry : IRequest<ResponceDto>
    {
        public int Id { get; set; }
    }
    public class GetAllAppointmentsQuerryHandler : IRequestHandler<GetAllAppointmentsQuerry, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public GetAllAppointmentsQuerryHandler(IAppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }
        public async Task<ResponceDto> Handle(GetAllAppointmentsQuerry request, CancellationToken cancellationToken)
        {
            var userType = await _appDbContext.Set<UserDetails>()
                .FirstOrDefaultAsync((x) => x.Id == request.Id);



            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);

            if (userType != null)
            {
                if (userType.UserType_ID == 2)
                {
                    const string sql = @"
select a.* ,
 CONCAT(upat.FirstName, ' ', upat.LastName) AS PatientFullName,
	CONCAT(upract.FirstName, ' ', upract.LastName) AS PractionerFullName,
	sd.SpecialisationType as practionerSpecialisation,
	upract.Profile as profile
from appointments a
left join Users upat on upat.id=a.PatientId
left join Users upract on upract.id=a.ProviderId
left join SpecialisationDetails sd on sd.Id=upract.Specialisation_ID
where (a.PatientId=@id or a.ProviderId=@id) and a.AppointmentStatus='Scheduled'";

                    var parameters = new { id = request.Id };
                    var appointments = await connection.QueryAsync<GetAllAppointmentDto>(sql, parameters);

                    return new ResponceDto
                    {
                        isSuccess = true,
                        data = appointments
                    };
                }

                else
                {


                    const string sql = @"
select a.* ,
 CONCAT(upat.FirstName, ' ', upat.LastName) AS PatientFullName,
	CONCAT(upract.FirstName, ' ', upract.LastName) AS PractionerFullName,
	sd.SpecialisationType as practionerSpecialisation,
	upract.Profile as profile
from appointments a
left join Users upat on upat.id=a.PatientId
left join Users upract on upract.id=a.ProviderId
left join SpecialisationDetails sd on sd.Id=upract.Specialisation_ID
where a.PatientId=@id or a.ProviderId=@id";

                    var parameters = new { id = request.Id };
                    var appointments = await connection.QueryAsync<GetAllAppointmentDto>(sql, parameters);

                    return new ResponceDto
                    {
                        isSuccess = true,
                        data = appointments
                    };
                }

            }
            else
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong please try again after some time"
                };







                //var allApointments= await _appDbContext.Set<Appointments>()
                //    .Where((x)=>x.PatientId==request.Id||x.ProviderId==request.Id).ToListAsync();

                //return new ResponceDto
                //{
                //    isSuccess = true,
                //    data=allApointments
                //};
                // throw new NotImplementedException();
            }
        }
    }
}
