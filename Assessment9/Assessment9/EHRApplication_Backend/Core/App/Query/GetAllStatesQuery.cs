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
    public class GetAllStatesQuery:IRequest<ResponceDto>
    {
    }
    public class GetAllStatesQueryHandler : IRequestHandler<GetAllStatesQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllStatesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponceDto> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
        {

            var states = await _appDbContext.Set<State>().ToListAsync();
            return new ResponceDto
            {
                isSuccess = true,
                data = states
            };


           // throw new NotImplementedException();
        }
    }
}
