using ClosedXML.Excel;
using IuCatalogHub.Components.Auth;
using Microsoft.JSInterop;
using Models;
using MudBlazor;
using System.Globalization;
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
        public bool _downLoadin = false;
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
            await CargarProductos();
            _verListado = !_verDetalle;
            productSelected = null;
            StateHasChanged();
        }

        private void CancelarEdicion()
        {
            _verDetalle = false;
            _verListado = !_verDetalle;
            productSelected = null;

        }


        private async Task AbrirTabEdicionProducto(Products item)
        {
            item.Commission = Math.Round(item.SalePrice * (item.PercentageCommission / 100),3);


            productSelected = item;
            productSelected.UserId = usuLogueado.UserId;
            _verListado = false;
            _verDetalle = !_verDetalle;
            StateHasChanged();
        }

        private async Task ExportarToExcel_Onclick()
        {
            _downLoadin = true;
            await Task.Delay(1);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Productos");

            // Encabezados
            worksheet.Cell(1, 1).Value = "IdProduct";
            worksheet.Cell(1, 2).Value = "ItemID";
            worksheet.Cell(1, 3).Value = "ProductName";
            worksheet.Cell(1, 4).Value = "Store";
            worksheet.Cell(1, 5).Value = "Mattel";
            worksheet.Cell(1, 6).Value = "UPC";
            worksheet.Cell(1, 7).Value = "Weight";
            worksheet.Cell(1, 8).Value = "Length";
            worksheet.Cell(1, 9).Value = "Width";
            worksheet.Cell(1, 10).Value = "Height";
            worksheet.Cell(1, 11).Value = "CostPrice";
            worksheet.Cell(1, 12).Value = "SalePrice";
            worksheet.Cell(1, 13).Value = "WFS";
            worksheet.Cell(1, 14).Value = "StorageFee_WFS";
            worksheet.Cell(1, 15).Value = "FactorPoundFeeShipment";
            worksheet.Cell(1, 16).Value = "Miscelaneous_Shipment";
            worksheet.Cell(1, 17).Value = "PercentageCommission";
            worksheet.Cell(1, 18).Value = "IsActive";
            worksheet.Cell(1, 19).Value = "IdImage";

            // Estilo encabezados
            worksheet.Range(1, 1, 1, 19).Style.Font.Bold = true;
            worksheet.Range(1, 1, 1, 19).Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;

            foreach (var p in lsProductos)
            {
                worksheet.Cell(row, 1).Value = p.IdProduct;
                worksheet.Cell(row, 2).Value = p.ItemID;
                worksheet.Cell(row, 3).Value = p.ProductName;
                worksheet.Cell(row, 4).Value = p.Store;
                worksheet.Cell(row, 5).Value = p.Mattel;
                worksheet.Cell(row, 6).Value = p.UPC;
                worksheet.Cell(row, 7).Value = p.Weight;
                worksheet.Cell(row, 8).Value = p.Length;
                worksheet.Cell(row, 9).Value = p.Width;
                worksheet.Cell(row, 10).Value = p.Height;
                worksheet.Cell(row, 11).Value = p.CostPrice;
                worksheet.Cell(row, 12).Value = p.SalePrice;
                worksheet.Cell(row, 13).Value = p.WFS;
                worksheet.Cell(row, 14).Value = p.StorageFee_WFS;
                worksheet.Cell(row, 15).Value = p.FactorPoundFeeShipment;
                worksheet.Cell(row, 16).Value = p.Miscelaneous_Shipment;
                worksheet.Cell(row, 17).Value = p.PercentageCommission;
                worksheet.Cell(row, 18).Value = p.IsActive;
                worksheet.Cell(row, 19).Value = p.IdImage;

                row++;
            }

            // Formatos numéricos
            worksheet.Columns(7, 17).Style.NumberFormat.Format = "#,##0.00";

            // Autoajustar columnas
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"maestro_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            await DescargarArchivo(stream.ToArray(), fileName);
            _downLoadin = false;
        }

        private async Task DescargarArchivo(byte[] archivo, string nombreArchivo)
        {
            await JS.InvokeVoidAsync(
                "downloadFile",
                nombreArchivo,
                Convert.ToBase64String(archivo)
            );
        }


    }
}
