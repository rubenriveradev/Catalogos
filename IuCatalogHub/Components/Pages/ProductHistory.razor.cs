using IuCatalogHub.Components.Auth;
using Models;
using MudBlazor;

namespace IuCatalogHub.Components.Pages
{
    public partial class ProductHistory
    {
        public bool _verListado = false;
        public bool _verHistorico = false;
        public bool _loading = true;
        private UserModel usuLogueado = new();
        List<Products> lsProductos = new List<Products>();
        private Products productSelected = null;
        private MudTable<Products> tableRef;

        List<EnvioDetailModel> lsProductHistorico = new List<EnvioDetailModel>();


        private string _cadenaBuscar = "";



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
                await CargarProductos();
                _verListado = true;



                _loading = false;
            }
            StateHasChanged();

        }


        private async Task Regresar_onClick()
        {
            navigation.NavigateTo("/", true);
        }
        private async Task CargarProductos()
        {
            lsProductos = new List<Products>();

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

        private async Task<TableData<Products>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
            if (lsProductos.Count == 0)
            {
                await CargarProductos();
            }
            IEnumerable<Products> data = lsProductos;
            if (!string.IsNullOrWhiteSpace(_cadenaBuscar))
            {
                data = data.Where(x =>
                    (x.ProductName?.Contains(_cadenaBuscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.ItemID?.Contains(_cadenaBuscar, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.UPC?.Contains(_cadenaBuscar, StringComparison.OrdinalIgnoreCase) ?? false));
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
        private void TriggerReload()
        {
            tableRef?.ReloadServerData();
        }


        private async Task VerHistorial_onClick(Products item)
        {
            lsProductHistorico = new List<EnvioDetailModel>();  
            var resp = await _serEnvios.ConsultarProductosHistorico(item.IdProduct);
            if (!resp.IsError)
            {
                lsProductHistorico = resp.Info;
                if (lsProductHistorico.Count > 0)
                {
                    _verListado = false;
                    _verHistorico = true;
                }
                else
                {
                    _snackBar.Add("No existen registros históricos para el producto.", Severity.Info, c => c.SnackbarVariant = Variant.Outlined);
                    return;
                }

            }
            else
            {
                _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                return;
            }
        }

        private async Task Volver_onClick()
        {
            _verListado = true;
            _verHistorico = false;
        }




    }
}
