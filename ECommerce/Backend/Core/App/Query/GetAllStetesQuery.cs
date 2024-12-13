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
    public  class GetAllStetesQuery:IRequest<ResponceDto>
    {
        public int Id { get; set; }
    }
    public class GetAllStetesQueryHandler : IRequestHandler<GetAllStetesQuery, ResponceDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllStetesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(GetAllStetesQuery request, CancellationToken cancellationToken)
        {

            var states = await _appDbContext.Set<State>().Where((x) => x.Country_Id == request.Id).ToListAsync();

            return new ResponceDto
            {
                isSuccess = true,
                data = states
            };


            //throw new NotImplementedException();
        }
    }
}
