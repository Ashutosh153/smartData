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
    public class GetAllSpecilizationQuery:IRequest<ResponceDto>
    {
    }

    public class GetAllSpecilizationQueryHandler:IRequestHandler<GetAllSpecilizationQuery,ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllSpecilizationQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(GetAllSpecilizationQuery request, CancellationToken cancellationToken)
        {
           var allSpecilizations= await _appDbContext.Set<Specialisation>().ToListAsync();
            return new ResponceDto
            {
                isSuccess = true,
                data = allSpecilizations
            };


        }
    }
}
