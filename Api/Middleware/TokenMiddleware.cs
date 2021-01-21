using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Domain.Srbac;
using Domain.Token;
using Infrastructure.Repositories;

namespace Api.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IGenericRepository tokenRepository)
        {
            if (context.User.FindFirstValue(ClaimTypes.Sid) == null)
            {
                await _next(context);

                return;
            }
            var hasTokenIsActive = tokenRepository.GetOne<TokenModel>(
                p => 
                    p.UserId == Guid.Parse(context.User.FindFirstValue(ClaimTypes.Sid))
                     && p.Role == Enum.Parse<SrbacRoles>(context.User.FindFirstValue(ClaimTypes.Role))
                     && p.Id == Guid.Parse(context.User.FindFirstValue(ClaimTypes.Name))
                     && p.IsActive == true
            ) != null;
            
            if(!hasTokenIsActive)
                context.Response.StatusCode = 401;
            else
                await _next(context);
        }
    }
}