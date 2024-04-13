using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResaleRarities.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Get UserSession from secure storage
                string getUserSessionFromStorage = await SecureStorage.Default.GetAsync("UserSession");
                if (string.IsNullOrEmpty(getUserSessionFromStorage))
                    return await Task.FromResult(new AuthenticationState(anonymous));

                // Deserialize into a UserSession object
                var deserializedUserSession = JsonSerializer.Deserialize<UserSession>(getUserSessionFromStorage);

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, deserializedUserSession.FullName!),
                    new Claim(ClaimTypes.Email, deserializedUserSession.Email!),
                    new Claim(ClaimTypes.Role, deserializedUserSession.UserRole!)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CustomAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }


        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (!string.IsNullOrEmpty(userSession.FullName) || !string.IsNullOrEmpty(userSession.Email) || !string.IsNullOrEmpty(userSession.UserRole))
            {
                string serializeUserSession = JsonSerializer.Serialize(userSession);
                await SecureStorage.Default.SetAsync("UserSession", serializeUserSession);

                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.FullName!),
                    new Claim(ClaimTypes.Email, userSession.Email!),
                    new Claim(ClaimTypes.Role, userSession.UserRole!)
                }));
            }
            else
            {
                SecureStorage.Default.Remove("UserSession");
                claimsPrincipal = anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
