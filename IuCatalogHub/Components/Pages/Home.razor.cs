namespace IuCatalogHub.Components.Pages
{
    public partial class Home
    {
        public bool verLogin = false;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var estadoAuth = await AuthProvider.GetAuthenticationStateAsync();
                var user = estadoAuth.User;
                if (!user.Identity.IsAuthenticated)
                {
                    verLogin = true;
                }
            }
            StateHasChanged();
        }


        public async Task btnGo_To(string url)
        {
            navigator.NavigateTo(url, forceLoad: true);
        }


    }
}
