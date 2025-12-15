using IuCatalogHub.Components.Auth;
using IuCatalogHub.Components.Modal;
using Models;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace IuCatalogHub.Components.Pages
{
    public partial class ComEnvio
    {
        private UserModel usuLogueado = new();
        List<EnvioModel> lsDespachos = new List<EnvioModel>();
        private EnvioModel despachoSelected = null;
        private MudTable<EnvioModel> tableRef;

        public bool _verListado = false;
        public bool _verDetalle = false;
        public bool _loading = true;
        private string _cadenaBuscar = "";

        private readonly DialogOptions _backdropClick = new() { BackdropClick = false };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var estadoAuth = await _authProvider.GetAuthenticationStateAsync();
                var user = estadoAuth.User;
                if (user.Identity.IsAuthenticated)
                {
                    usuLogueado = estadoAuth.User.ToUsuarioModel();
                }
                await CargarDespachos();
                _verListado = true;
                _verDetalle = !_verListado;


                _loading = false;
            }
            StateHasChanged();

        }

        private async Task Regresar_onClick()
        {
            navigation.NavigateTo("/", true);
        }


        private async Task AbrirTabNewProducto()
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", "Nuevo Envío");

            var dialog = await _dialogServicio.ShowAsync<MdEnvios>("Nuevo Envío", parameters);

            // Si quieres esperar el resultado del modal:
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                // Aquí puedes manejar lo que el usuario envió desde el modal
            }


            StateHasChanged();
        }





        private async Task<TableData<EnvioModel>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
            if (lsDespachos.Count == 0)
            {
                await CargarDespachos();
            }
            IEnumerable<EnvioModel> data = lsDespachos;
            if (!string.IsNullOrWhiteSpace(_cadenaBuscar))
            {
                data = data.Where(x =>
                    (x.IdDespacho.ToString()?.Contains(_cadenaBuscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.FechaDespacho?.Contains(_cadenaBuscar, StringComparison.OrdinalIgnoreCase) ?? false));
            }
            var pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();

            return await Task.FromResult(new TableData<EnvioModel>
            {
                TotalItems = data.Count(),
                Items = pagedData
            });
        }
        private void TriggerReload()
        {
            tableRef?.ReloadServerData();
        }
        private async Task CargarDespachos()
        {
            var resp = await _serEnvios.ConsultarEnvios(0);
            if (!resp.IsError)
            {
                lsDespachos = resp.Info;
            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }
            _loading = false;
        }

        private async Task AbrirTabNewEnvio()
        {
            var opciones = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium, // Puedes usar Small, Medium, Large, ExtraLarge
                FullWidth = true,           // Ocupa todo el ancho disponible dentro del MaxWidth
                BackdropClick = false       // bloquea el cierre al hacer clic fuera del diálogo
            };

            EnvioModel miEnv = new EnvioModel();
            miEnv.UsserId = usuLogueado.UserId;
            miEnv.IdEstado = 1; // Estado Activo por defecto


            var parametros = new DialogParameters { ["_envioModel"] = miEnv };

            var dialog = await _dialogServicio.ShowAsync<MdEnvios>("Nuevo Envio", parametros, opciones);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                _verListado = false;
                await CargarDespachos();
                _verListado = true;
                StateHasChanged();
            }
        }
        private async Task AbrirTabEdicionDespacho(EnvioModel item)
        {
            var opciones = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                BackdropClick = false
            };

            var parametros = new DialogParameters { ["_envioModel"] = item };            
            var dialog = await _dialogServicio.ShowAsync<MdEnvios>("Edicion Envio", parametros, opciones);            
            var result = await dialog.Result;            
            if (!result.Canceled)
            {
                _verListado = false;
                
               // _verListado = true;
               
            }
            lsDespachos = new List<EnvioModel>();
            _verListado = false;
            await CargarDespachos();
            StateHasChanged();
            _verListado = true;
        }

        private async Task AbrirAdicionProductosEnvio( int idEnvio)
        {
            navigation.NavigateTo($"/envio-producto/{idEnvio}",true);
        }







    }
}
