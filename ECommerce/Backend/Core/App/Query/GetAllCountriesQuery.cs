using App.Core.Model;
using Core.Interface;
using Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Query
{
    public class GetAllCountriesQuery:IRequest<ResponceDto>
    {
    }
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllCountriesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _appDbContext.Set<Country>().ToListAsync();
            return new ResponceDto
            {
                isSuccess = true,
                data=countries

            };
                


           
        }
    }
}
