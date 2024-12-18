using App.Core.ModelsDto;
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
    public class GetAllSoapNotesById:IRequest<ResponceDto>
    {
        public int Id {  get; set; }
    }
    public class GetAllSoapNotesByIdHandler : IRequestHandler<GetAllSoapNotesById, ResponceDto>
    {
        private readonly IConfiguration _configuration;
        public GetAllSoapNotesByIdHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ResponceDto> Handle(GetAllSoapNotesById request, CancellationToken cancellationToken)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);
            const string sql = @"
                    select sn.* ,
                    a.ChiefComplaint,
                    a.appointmentDate,
                    CONCAT(upro.FirstName,' ',upro.LastName) as providerName,
                    CONCAT(upat.FirstName,' ',upat.LastName) as patientName
                    from soapNotes sn
                    left join appointments a on a.Id=sn.AppointmnetID
                    left join users upat on a.PatientId=upat.id
                    left join Users upro on a.ProviderId=upro.id
                    where a.PatientId=@id or a.ProviderId=@id";

            var parameters = new { id = request.Id };
            var appointments = await connection.QueryAsync<GetAllSoapNotesDto>(sql, parameters);

            return new ResponceDto
            {
                isSuccess = true,
                data = appointments
            };
        }
    }
}
