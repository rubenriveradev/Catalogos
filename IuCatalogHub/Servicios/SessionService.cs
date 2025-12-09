using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace IuCatalogHub.Servicios
{
    public class SessionService
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly NavigationManager _navigationManager;

        public SessionService(ProtectedSessionStorage sessionStorage, NavigationManager navigationManager)
        {
            _sessionStorage = sessionStorage;
            _navigationManager = navigationManager;
        }

        public async Task LogoutAsync()
        {
            await _sessionStorage.DeleteAsync("usuario"); // o tu clave de sesión
            _navigationManager.NavigateTo("/", forceLoad: true);
        }


    }
}
