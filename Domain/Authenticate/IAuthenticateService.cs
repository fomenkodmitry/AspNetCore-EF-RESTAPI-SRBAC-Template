﻿using System;
using System.Threading.Tasks;
using Domain.Base;

 namespace Domain.Authenticate
{
    public interface IAuthenticationService
    {
        Task<ResultContainer<UserRegistrationResponseDto>> Register(UserRegistrationRequestDto requestDto);
        Task<ResultContainer<UserLoginResponseDto>> Login(UserLoginRequestDto requestDto);
        Task<ResultContainer<bool>> Logout(Guid sessionId);
        Task<ResultContainer<bool>> SavePushToken(Guid sessionId, string pushToken);

    }
}