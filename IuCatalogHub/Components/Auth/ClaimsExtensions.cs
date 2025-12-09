using Models;
using System.Security.Claims;

namespace IuCatalogHub.Components.Auth
{
    public static class ClaimsExtensions
    {
        public static UserModel ToUsuarioModel(this ClaimsPrincipal user)
        {
            return new UserModel
            {
                UserId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0,
                FirstName = user.FindFirstValue(ClaimTypes.Name) ?? "",
                RoleId = int.Parse(user.FindFirstValue(ClaimTypes.Role)),
                UserName = user.FindFirstValue("UserName") ?? ""

            };
        }
    }
}
