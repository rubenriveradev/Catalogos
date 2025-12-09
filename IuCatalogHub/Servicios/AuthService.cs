using IuCatalogHub.Components.Auth;
using LnCatalogHub.Login;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Models;

namespace IuCatalogHub.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly AuthenticationStateProvider _authProvider;
        private const string SessionKey = "usuario";
        private readonly SerUser _serUsuarioRepo;
        public AuthService(SerUser usuarioRepo,
                           ProtectedSessionStorage sessionStorage,
                           AuthenticationStateProvider authProvider)
        {
            _sessionStorage = sessionStorage;
            _authProvider = authProvider;
            _serUsuarioRepo = usuarioRepo;
        }

        public async Task<Response<bool>> LoginAsync(string username, string password)
        {
            try
            {
                var resp = await _serUsuarioRepo.ValidarUsuarioAsync(username, password);
                if (resp == null)
                {
                    return new Response<bool>(true, "Usuario o clave incorrectos", false);
                }
                else
                {
                    if (resp.IsError)
                    {
                        return new Response<bool>(true, resp.MensajeError, false);
                    }
                    else
                    {
                        var usuario = resp.Info;
                        await _sessionStorage.SetAsync(SessionKey, usuario);
                        ((CustomAuthStateProvider)_authProvider).NotifyUserAuthentication(usuario!);

                        return new Response<bool>(false, string.Empty, true);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response<bool>(true, ex.Message, false);
            }



        }
        public async Task LogoutAsync()
        {
            await _sessionStorage.DeleteAsync(SessionKey);
            ((CustomAuthStateProvider)_authProvider).NotifyUserLogout();
        }

        public async Task<UserModel> GetUsuarioActualAsync()
        {
            var result = await _sessionStorage.GetAsync<UserModel>(SessionKey);
            return result.Success ? result.Value : null;
        }


    }
}
