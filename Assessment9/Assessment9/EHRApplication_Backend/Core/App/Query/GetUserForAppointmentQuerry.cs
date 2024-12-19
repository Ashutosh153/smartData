using App.Core.ModelsDto;
using Core.Interface;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class GetUserForAppointmentQuerry:IRequest<ResponceDto>
    {
        public int id { get; set; }
    }
    
    public class GetUserForAppointmentQuerryHandler:IRequestHandler<GetUserForAppointmentQuerry,ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration; 
        public GetUserForAppointmentQuerryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponceDto> Handle(GetUserForAppointmentQuerry request, CancellationToken cancellationToken)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);

            const string sql = @"
select a.AppointmentTime,
a.AppointmentDate,
a.ChiefComplaint,
concat(u.FirstName,' ',u.LastName) as PatientName,
u.DateOfBirth as dob,
u.Gender as gender,
u.Profile
from appointments a
left join Users u on a.PatientId=u.Id 
where a.id=@id ";

            var parameters = new { id = request.id };
            var userDetails = await connection.QueryAsync<GetUserForAppointmentDto>(sql, parameters);



            return new ResponceDto
            {
                isSuccess = true,
                data = userDetails
            };
        }
    }
}
