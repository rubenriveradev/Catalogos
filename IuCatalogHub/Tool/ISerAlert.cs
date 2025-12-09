
namespace IuCatalogHub.Tool
{
    public interface ISerAlert
    {
        Task<bool> Confirmar(string titulo, string mensaje, TipoMsg tipoAlerta);
        Task<bool> Confirmar(object param);
        Task MsgAlert(string titulo, string mensaje, TipoMsg tipoMensaje);
    }
}
