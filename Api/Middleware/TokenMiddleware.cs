using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Domain.Srbac;
using Infrastructure.Repositories.Token;

namespace Api.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,TokenRepository tokenRepository)
        {
            if (context.User.FindFirstValue(ClaimTypes.Sid) == null)
            {
                await _next(context);

                return;
            }
            var hasTokenIsActive = await tokenRepository.HasActiveToken(
                Guid.Parse(context.User.FindFirstValue(ClaimTypes.Sid)),
                Guid.Parse(context.User.FindFirstValue(ClaimTypes.Name)),
                Enum.Parse<SrbacRoles>(context.User.FindFirstValue(ClaimTypes.Role))
            );
            if(!hasTokenIsActive)
                context.Response.StatusCode = 401;
            else
                await _next(context);
        }
    }
}