using IuCatalogHub.Components.Auth;
using Models;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace IuCatalogHub.Components.Pages
{
    public partial class ComProducto
    {
        private UserModel usuLogueado = new();
        List<Products> lsProductos = new List<Products>();
        private Products productSelected = null;
        private MudTable<Products> tableRef;

        public bool _verListado = false;
        public bool _verDetalle = false;


        public bool _loading = true;

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
                _verDetalle = !_verListado;


                _loading = false;
            }
            StateHasChanged();

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
        private async Task AbrirTabNewProducto()
        {
            productSelected = new Products();
            productSelected.UserId = usuLogueado.UserId;
            _verListado = false;
            _verDetalle = !_verDetalle;
            StateHasChanged();
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


        private async Task GuardarProducto(Products product)
        {
            _verDetalle = false;
            _verListado = !_verDetalle;
            productSelected = null;
        }

        private void CancelarEdicion()
        {
            _verDetalle = false;
            _verListado = !_verDetalle;
            productSelected = null;

        }


        private async Task AbrirTabEdicionProducto(Products item)
        {
            productSelected = item;
            productSelected.UserId = usuLogueado.UserId;
            _verListado = false;
            _verDetalle = !_verDetalle;
            StateHasChanged();
        }

        




    }
}
