using IuCatalogHub.Components;
using IuCatalogHub.Components.Auth;
using IuCatalogHub.Servicios;
using IuCatalogHub.Tool;
using LnCatalogHub.Catalogos;
using LnCatalogHub.Despachos;
using LnCatalogHub.Login;
using LnCatalogHub.Producto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using MudExtensions.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices(
    config =>
    {
        //config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;

        config.SnackbarConfiguration.PreventDuplicates = false;
        config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.VisibleStateDuration = 2000;
        config.SnackbarConfiguration.HideTransitionDuration = 1000;
        config.SnackbarConfiguration.ShowTransitionDuration = 1000;
        //config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    }
);

builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddMudExtensions();

builder.Services.AddTransient<ISerAlert, SerAlert>();
builder.Services.AddTransient<SerUser>();
builder.Services.AddTransient<SerProduct>();
builder.Services.AddTransient<SerEnvio>();
builder.Services.AddTransient<SerCatalogo>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
