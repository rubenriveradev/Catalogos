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




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CargarEnvios(id);
                await CargarProductos();

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

        }











    }
}
