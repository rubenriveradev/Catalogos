using Models;

namespace IuCatalogHub.Servicios
{
    public interface IAuthService
    {
        Task LogoutAsync();
        Task<UserModel> GetUsuarioActualAsync();
        Task<Response<bool>> LoginAsync(string username, string password);
    }
}
