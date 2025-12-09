using LnCatalogHub.Despachos;
using Microsoft.AspNetCore.Components;
using Models;
using MudBlazor;

namespace IuCatalogHub.Components.Pages
{
    public partial class EnvioProducto
    {

        [Parameter] public int id { get; set; }
        private UserModel usuLogueado = new();
        List<EnvioModel> lsEnvios = new List<EnvioModel>();
        private EnvioModel envioSelected = null;
        public bool _loading = true;





        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CargarEnvios(id);

            }
            StateHasChanged();
        }

        private async Task CargarEnvios(int id)
        {
            var resp = await _serEnvios.ConsultarEnvios(id);
            if (!resp.IsError)
            {
                lsEnvios = resp.Info;
                if(lsEnvios.Count > 0)
                {
                    envioSelected = lsEnvios.FirstOrDefault();
                }
                

            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }
            _loading = false;
        }
    }
}
