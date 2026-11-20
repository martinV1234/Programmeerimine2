using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Authentication;
using KooliProjekt.Application.Authentication.Jwt;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;

namespace KooliProjekt.Application.Features.Authentication
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, OperationResult<string>>
    {
        private const string InvalidCredentialsError = "Invalid e-mail or password.";

        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenBuilder _tokenBuilder;

        public AuthenticateCommandHandler(ApplicationDbContext dbContext, JwtTokenBuilder tokenBuilder)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenBuilder = tokenBuilder ?? throw new ArgumentNullException(nameof(tokenBuilder));
        }

        public async Task<OperationResult<string>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new OperationResult<string>();
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrEmpty(request.Password))
            { response.AddError(InvalidCredentialsError); return response; }
            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Email == request.UserName);

            if (user == null || !PasswordHelper.Verify(request.Password, user.Password))
            {
                response.AddError(InvalidCredentialsError);

                return response;
            }

            if (!user.Password.StartsWith("PBKDF2$"))
            {
                user.Password = PasswordHelper.Hash(request.Password);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };
            if (user.IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            var token = _tokenBuilder.GenerateToken(claims.ToArray());

            response.Value = token;

            return response;
        }
    }
}
