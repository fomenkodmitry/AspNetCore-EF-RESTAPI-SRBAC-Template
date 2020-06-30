﻿using System;
 using System.Collections.Generic;
 using System.Linq;
using System.Threading.Tasks;
 using Domain.Srbac;
 using Domain.Token;
 using Infrastructure.AppSettings;
 using Infrastructure.Contexts;
 using Microsoft.EntityFrameworkCore;

 namespace Infrastructure.Repositories.Token
{
    public class TokenRepository: BaseRepository<TokenModel>
    {
        public TokenRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }
        public IEnumerable<string> GetAllActivePushTokenByUser(Guid userId, SrbacRoles role)
        {
            return Context.Tokens
                .Where(p => p.UserId == userId && p.PushToken != null && p.IsActive == true && p.Role == role)
                .Select(p => p.PushToken);
        }

        public Task<bool> HasActiveToken(Guid tokenId, Guid userId, SrbacRoles role)
        {
            return Context.Tokens
                .Where(p => p.UserId == userId && p.Role == role && p.Id == tokenId && p.IsActive == true)
                .AnyAsync();
        }

        public async Task ChangeTokenActivityForUser(Guid userId, SrbacRoles role, bool isActive)
        {
            var tokens = Context.Tokens.Where(f => f.UserId == userId && f.Role == role);
            await tokens.ForEachAsync(a=> a.IsActive = isActive);
            await Context.SaveChangesAsync();
        }
        public async Task ChangeTokenActivityForUser(Guid userId, bool isActive)
        {
            var tokens = Context.Tokens.Where(f => f.UserId == userId);
            await tokens.ForEachAsync(a=> a.IsActive = isActive);
            await Context.SaveChangesAsync();
        }
    }
}