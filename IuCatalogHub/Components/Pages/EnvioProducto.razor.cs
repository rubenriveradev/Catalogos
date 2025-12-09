using Microsoft.AspNetCore.Components;
using Models;

namespace IuCatalogHub.Components.Pages
{
    public partial class EnvioProducto
    {

        [Parameter] public int id { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                

            }
            StateHasChanged();
        }





    }
}
