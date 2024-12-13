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

namespace App.Core.App.Query
{
    public class GetAllCitiesQuerry:IRequest<ResponceDto>
    {
        public int id {  get; set; }
    }
    public class GetAllCitiesQuerryHandler:IRequestHandler<GetAllCitiesQuerry,ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllCitiesQuerryHandler(IAppDbContext appDbContext)
        {
               _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(GetAllCitiesQuerry request, CancellationToken cancellationToken)
        {


            if (request.id == null)
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "something went wrong",

                };
            }
            else
            {
                var cities = await _appDbContext.Set<Cities>()
                    .Where((x) => x.StateId == request.id).ToListAsync();

                return new ResponceDto
                {
                    isSuccess = true,

                    data = cities
                };
            }

           
        }
    }

}
