using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using KooliProjekt.Application.Authentication;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var entity = new User();
            if (request.Id == 0)
            {
                _dbContext.Users.Add(entity);
            }
            else
            {
                entity = await _dbContext.Users.SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return result.AddPropertyError("Id", "Cannot find record with ID " + request.Id);
                }
            }
            entity.Name = request.Name;
            entity.Phone = request.Phone;
            entity.Email = request.Email;
            entity.IsAdmin = request.IsAdmin;
            entity.Password = PasswordHelper.Hash(request.Password);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
