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
    public class ValidateCardDetailsQuery:IRequest<ResponceDto>
    {
        public CardDetailsDto CardDetails { get; set; }
    }
    public class ValidateCardDetailsQueryHandler : IRequestHandler<ValidateCardDetailsQuery, ResponceDto>
    {
        public readonly IAppDbContext _appDbContext;
        public ValidateCardDetailsQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponceDto> Handle(ValidateCardDetailsQuery request, CancellationToken cancellationToken)
        {

            var validateCard = await _appDbContext.Set<Card>()
                .FirstOrDefaultAsync((x) => x.CardNumber == request.CardDetails.cardNumber
                                            && x.ExpiryDate==request.CardDetails.expiryDate
                                            && x.CVV==request.CardDetails.cvv);

            if (validateCard != null)
            {

                return new ResponceDto
                {
                    isSuccess = true,
                    message = "payment successfull"

                };
            }

            else {
                return new ResponceDto
                {
                    isSuccess = false,
                    message = "invalid card credentials"
                };
            }


        }
    }
}
