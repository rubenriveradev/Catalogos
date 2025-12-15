using IuCatalogHub.Components.Auth;
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


        string _productoBuscar;
        private Products productSelected = null;
        private MudTable<Products> tableRef;
        Color Color = Color.Success;

        List<EnvioDetailModel> lsProductosEnvio = new List<EnvioDetailModel>();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var estadoAuth = await _authProvider.GetAuthenticationStateAsync();
                var user = estadoAuth.User;
                if (user.Identity.IsAuthenticated)
                {
                    usuLogueado = estadoAuth.User.ToUsuarioModel();
                    await CargarEnvios(id);
                    await CargarProductosEnvio(id);
                    await CargarProductos();

                }
                _loading = false;
            }




            StateHasChanged();
        }

        private async Task CargarProductosEnvio(int id)
        {
            var resp = await _serEnvios.ConsultarEnvioDetalle(id);
            if (!resp.IsError)
            {
                lsProductosEnvio = resp.Info;
            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }
            _loading = false;

        }

        private async Task Regresar_onClick()
        {
            navigation.NavigateTo("/Shipment",true);
        }


        private async Task CargarEnvios(int id)
        {
            var resp = await _serEnvios.ConsultarEnvios(id);
            if (!resp.IsError)
            {
                lsEnvios = resp.Info;
                if (lsEnvios.Count > 0)
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

        List<Products> lsProductos = new List<Products>();


        private async Task<TableData<Products>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
            if (lsProductos.Count == 0)
            {
                await CargarProductos();
            }
            IEnumerable<Products> data = lsProductos;
            if (!string.IsNullOrWhiteSpace(_productoBuscar))
            {
                data = data.Where(x =>
                    (x.ProductName?.Contains(_productoBuscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.ItemID?.Contains(_productoBuscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.UPC?.Contains(_productoBuscar, StringComparison.OrdinalIgnoreCase) ?? false));
            }
            var pagedData = data
                .Skip(state.Page * state.PageSize)
                .Take(state.PageSize)
                .ToList();

            return await Task.FromResult(new TableData<Products>
            {
                TotalItems = data.Count(),
                Items = pagedData
            });


        }
        private async Task CargarProductos()
        {
            var resp = await _serProductos.ConsultarProductos(0);
            if (!resp.IsError)
            {
                lsProductos = resp.Info;
            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }
            _loading = false;
        }

        private void TriggerReload()
        {
            tableRef?.ReloadServerData();
        }

        private async Task AdicionarProductoEnvio(Products prod)
        {
           var resp = await _serEnvios.CreaActualizaEnvioDetalle(0, id, prod.IdProduct, 1, usuLogueado.UserId);
            if (!resp.IsError)
            {
                _snackBar.Add("Producto adicionado correctamente", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                await CargarProductosEnvio(id);
            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }
        }


        private async Task QuitarProducto(int idDetalle)
        {
            var resp = await _serEnvios.EliminarEnvioDetalle(idDetalle);
            if (!resp.IsError)
            {
                _snackBar.Add("Producto eliminado correctamente", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                await CargarProductosEnvio(id);
            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }
        }

        private void OnScroll(ScrollEventArgs e)
        {
            Color = (e.FirstChildBoundingClientRect.Top * -1) switch
            {
                var x when x < 500 => Color.Primary,
                var x when x < 1500 => Color.Secondary,
                var x when x < 2500 => Color.Tertiary,
                _ => Color.Error
            };
        }








    }
}
