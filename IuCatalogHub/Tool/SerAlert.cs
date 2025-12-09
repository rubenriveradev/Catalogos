using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace IuCatalogHub.Tool
{
    public class SerAlert: ISerAlert
    {
        private readonly IJSRuntime _js;
        public SerAlert(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> Confirmar(string titulo, string mensaje, TipoMsg tipoAlerta)
        {
            return await _js.InvokeAsync<bool>("CustomConfirm", titulo, mensaje, tipoAlerta.ToString());
        }
        public async Task<bool> Confirmar(object param)
        {
            var b = await _js.InvokeAsync<object>("Swal.fire", param);
            dynamic aa = JsonConvert.DeserializeObject(b.ToString());
            var dd = ((Newtonsoft.Json.Linq.JProperty)(((Newtonsoft.Json.Linq.JContainer)aa).First)).Value.ToString();
            return Convert.ToBoolean(dd.ToLower());
        }

        public async Task MsgAlert(string titulo, string mensaje, TipoMsg tipoMensaje)
        {
            await _js.InvokeVoidAsync("Swal.fire", titulo, mensaje, tipoMensaje.ToString());
        }

    }
    public enum TipoMsg
    {
        question, warning, error, success, info
    }
}
