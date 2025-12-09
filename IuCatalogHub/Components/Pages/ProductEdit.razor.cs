using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;
using MudBlazor;

namespace IuCatalogHub.Components.Pages
{
    public partial class ProductEdit
    {
        [Parameter] public Products Product { get; set; } = new();
        [Parameter] public EventCallback<Products> OnSave { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }

        private MudForm _form;

        string nomEstado = "Activo";
        private IBrowserFile selectedImage;


        private async Task Save()
        {
            await _form.Validate();
            if (_form.IsValid)
            {
                var resp = await _serProductos.GuardarProductos(Product);
                if (!resp.IsError)
                {
                    _snackBar.Add("Producto guardado correctamente", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                    await OnSave.InvokeAsync(Product);
                }
                else
                {
                    _snackBar.Add(resp.MensajeError, Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                    return;
                }                
            }
        }

        private async Task Cancel()
        {
            await OnCancel.InvokeAsync();
        }

        private void PercentageCommission_change(decimal? newValue)
        {
            Product.PercentageCommission = newValue ?? 0;
            Product.Commission = (Product.SalePrice * Product.PercentageCommission) / 100;
            Product.Commission = Math.Round((Product.SalePrice * Product.PercentageCommission) / 100,3);
        }
        private void SalePrice_change(decimal? newValue)
        {
            Product.SalePrice = newValue ?? 0;
            Product.Commission = (Product.SalePrice * Product.PercentageCommission) / 100;
            Product.Commission = Math.Round((Product.SalePrice * Product.PercentageCommission) / 100, 3);
        }

        private void Activo_onChange(bool newValue)
        {
            //var val = _rol;
            Product.IsActive = newValue;

            if (Product.IsActive)
            {
                nomEstado = "Activo";
            }
            else
            {
                nomEstado = "Inactivo";
            }
        }


        private async Task UploadFiles(IBrowserFile file)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            Product.ImageData = ms.ToArray();
        }



        private async Task OnImageSelected(InputFileChangeEventArgs e)
        {
            selectedImage = e.File;

            // Ejemplo: leer el archivo en memoria
            using var stream = selectedImage.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024); // 5 MB
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            // Guardar en tu modelo como byte[]
            Product.ImageData = ms.ToArray();
            
        }


    }
}
