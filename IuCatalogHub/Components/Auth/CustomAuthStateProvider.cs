using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Models;
using System.Security.Claims;

namespace IuCatalogHub.Components.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await _sessionStorage.GetAsync<UserModel>("usuario");

                if (!result.Success)
                    return new AuthenticationState(_anonymous);

                var usuario = result.Value;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.UserId.ToString()),
                    new Claim(ClaimTypes.Name, usuario.FirstName + " " + usuario.LastName),
                    new Claim(ClaimTypes.Role, usuario.RoleId.ToString()),
                    new Claim("NombreRol", usuario.RoleName.ToString()),
                    new Claim("UserName", usuario.UserName),
                };

                var identity = new ClaimsIdentity(claims, "apiauth");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public void NotifyUserAuthentication(UserModel usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.NameIdentifier, usuario.UserId.ToString()),
                    new Claim(ClaimTypes.Role, usuario.RoleId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, "apiauth");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }


    }
}
