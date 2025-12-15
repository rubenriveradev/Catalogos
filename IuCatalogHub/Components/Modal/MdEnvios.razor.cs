using LnCatalogHub.Despachos;
using Microsoft.AspNetCore.Components;
using Models;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace IuCatalogHub.Components.Modal
{
    public partial class MdEnvios
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; }

        [Parameter] public EnvioModel _envioModel { get; set; }
        //[Parameter] public string _titulo { get; set; }

        DateTime FechaEnvio = DateTime.Now;

        List<CatalogosModel> lsEstados = new List<CatalogosModel>();



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrWhiteSpace(_envioModel.FechaDespacho))
                {
                    FechaEnvio = DateTime.Parse(_envioModel.FechaDespacho);
                }
                await CargarEstados("EST_ENVIOS");
            }
            StateHasChanged();
        }

        

        private async Task CargarEstados(string v)
        {
            var resp = await _serCatalogo.ConsultarCatalogo(v);
            if (!resp.IsError)
            {
                lsEstados = resp.Info;
                if (lsEstados.Any())
                {
                    var item = new CatalogosModel { Codigo = 0, Descripcion = "--Seleccione" };
                    lsEstados.Insert(0, item);
                }
            }
        }

        private async Task OnDateChange(DateTime? newDate)
        {
            FechaEnvio = (DateTime)newDate;
        }

        private async Task Guardar()
        {

            _envioModel.FechaDespacho = FechaEnvio.ToString("yyyy-MM-dd");


            var resp = await _serEnvio.GuardarEnvios(_envioModel);
            if (!resp.IsError)
            {
                _snackBar.Add("Envio guardado correctamente", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }           
        }

        private async Task Cancelar()
        {
            MudDialog.Cancel();
        }
    }
}
