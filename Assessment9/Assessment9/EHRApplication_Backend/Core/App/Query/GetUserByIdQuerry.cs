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
    public  class GetUserByIdQuerry:IRequest<ResponceDto>
    {
        public int id {  get; set; }
    }
    public class GetUserByIdQuerryHandler : IRequestHandler<GetUserByIdQuerry, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public GetUserByIdQuerryHandler(IAppDbContext appDbContext,IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }
        public async Task<ResponceDto> Handle(GetUserByIdQuerry request, CancellationToken cancellationToken)
        {


            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);
            const string sql = @"
                                  select u.* ,

 sd.SpecialisationType as Specilisation,
 c.CityName as CityName,
 s.Name as StateName
 from Users u 
 left join SpecialisationDetails sd on sd.Id=u.Specialisation_ID
 left join Cities c on u.CityId=c.Id
 left join StateDetails s on s .Id=u.StateId
where u.id=@UserId";

            var parameters = new { UserId = request.id };
            var user = await connection.QueryAsync<UserProfileDto>(sql,parameters);



           // var user = await _appDbContext.Set<UserDetails>().FirstOrDefaultAsync((x) => x.Id == request.id);
            if (user == null) 
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong "
                };
            }
          
            return new ResponceDto
            {
                isSuccess = true,
                data = user
            };

            //throw new NotImplementedException();
        }
    }
}
