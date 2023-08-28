using ApiService.DTOs;
using AuthorizationLibrary.Models;
using JwtService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtService
{
    public interface IJwtService
    {
        string GenerateAccessTokenAsync(User user);
        RefreshToken GenerateRefreshToken(User user);
        Task AddRolesClaim(ref ClaimsIdentity claims);
        string GenerateRefreshToken();
        string RefreshAccessTokenAsync(RefreshToken? refreshTokenEntity, User user);
        ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true);

    }
}
