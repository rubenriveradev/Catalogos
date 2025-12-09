using IuCatalogHub.Tool;
using Microsoft.AspNetCore.Components;
using Models;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IuCatalogHub.Components.Layout
{
    public partial class Login
    {
        private LoginModel loginModel = new();
        private MudForm form;
        private bool showPassword;
        private bool isLoading;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            loginModel.Username = "rrivera@gmail.com";
            loginModel.Password = "12345";
            await Task.Delay(1);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity?.IsAuthenticated ?? false)
                {
                    StateHasChanged();
                    navigation.NavigateTo("/", true); // Ya autenticado, redirige
                }
            }
        }

        private async Task OnLogin()
        {
            //IsLoading = true;
            StateHasChanged();
            PasswordHelper tool = new PasswordHelper();
            string pass = tool.GetSHA512(loginModel.Password);

            loginModel.Password = pass;

            var resp = await _authService.LoginAsync(loginModel.Username, loginModel.Password);
            if (!resp.IsError)
            {
                navigation.NavigateTo("/", forceLoad: true);
            }
            else
            {
                loginModel.Password = string.Empty;
                //await _msg.MsgAlert("", resp.MensajeError, TipoMsg.warning);

            }
            //IsLoading = false;
        }



        private async Task HandleValidSubmit()
        {
            isLoading = true;
            StateHasChanged();

            try
            {
                if (form is not null)
                {
                    await form.Validate();
                }

                if (form?.IsValid == true)
                {
                    var success = await AuthenticateAsync(loginModel);
                    if (success)
                    {
                        Snackbar.Add("Inicio de sesión correcto", Severity.Success);
                        // Navegar a la página principal (ajusta la ruta según tu app)
                        navigation.NavigateTo("/");
                    }
                    else
                    {
                        Snackbar.Add("Usuario o contraseña incorrectos", Severity.Error);
                    }
                }
                else
                {
                    Snackbar.Add("Corrige los errores del formulario", Severity.Warning);
                }
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        // Simulación de autenticación: reemplaza por tu servicio real.
        private async Task<bool> AuthenticateAsync(LoginModel model)
        {
            await Task.Delay(6000); // simula petición de red
            // Ejemplo: cambia la lógica para integrar tu API o CustomAuthStateProvider
            return model.Username == "admin" && model.Password == "1234";
        }

        private void ToggleShowPassword()
        {
            showPassword = !showPassword;
        }

        private void OnValidated()
        {
            Console.WriteLine("Formulario validado");
        }
    }


}

