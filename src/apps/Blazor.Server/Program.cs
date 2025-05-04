
using Blazor.Common.Helpers;
using Blazor.Server.Authentication;
using Blazor.Server.Components;
using Blazor.Server.HttpClients;
using Blazor.Server.Services.Implementations;
using Blazor.Server.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Syncfusion
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXtedHZXRGZcVkVxWkBWYUA=");
builder.Services.AddSyncfusionBlazor();

// Local Storage
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<LocalStorageService>();


// Http Client
builder.Services.AddHttpClient<BaseHttpClient>((sp, client) =>
{
    //IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
#pragma warning disable S1075 // URIs should not be hardcoded
    client.BaseAddress = new Uri("https://system.webport.co.za");
#pragma warning restore S1075 // URIs should not be hardcoded
});

builder.Services.AddScoped<BaseHttpClient>();

// Services
builder.Services.AddScoped<ITokenService, TokenService>();

// Authentication
//builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

await app.RunAsync();
