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
    public class GetAllPractionerQuerry:IRequest<ResponceDto>
    {
        public int specilizationId { get; set; }
    }
    public class GetAllPractionerQuerryHandler : IRequestHandler<GetAllPractionerQuerry, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public GetAllPractionerQuerryHandler(IAppDbContext appDbContext,IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }
        public async Task<ResponceDto> Handle(GetAllPractionerQuerry request, CancellationToken cancellationToken)
        {
            

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);

            if (request.specilizationId == 0) {

                const string sql = @"
 select u.FirstName as FirstName,
		u.LastName as LastName,
		u.Profile as Profile,
		u.Qualification as Qualification,
		u.Registration_Number as Registration_Number,
		u.Visiting_Charge as Visiting_Charge,
		sd.SpecialisationType as SpecialisationType

 from users as u
 left join SpecialisationDetails sd on u.Specialisation_ID= sd.Id
 where u.UserType_ID=2
                                  ";

                // var parameters = new { UserId = request.id };
               var  users = await connection.QueryAsync<GetPractionerForAppointmentDto>(sql);

                return new ResponceDto
                {
                    isSuccess = true,
                    data = users
                };

            }
            else
            {
                const string sql = @"
 select u.FirstName as FirstName,
		u.LastName as LastName,
		u.Profile as Profile,
		u.Qualification as Qualification,
		u.Registration_Number as Registration_Number,
		u.Visiting_Charge as Visiting_Charge,
		sd.SpecialisationType as SpecialisationType

 from users as u
 left join SpecialisationDetails sd on u.Specialisation_ID= sd.Id
 where u.UserType_ID=2 and u.Specialisation_ID=@specId
                                  ";

                 var parameters = new { specId = request.specilizationId };
                var users = await connection.QueryAsync<GetPractionerForAppointmentDto>(sql,parameters);

                return new ResponceDto
                {
                    isSuccess = true,
                    data = users
                };

            }

         





         //   throw new NotImplementedException();
        }
    }
}
