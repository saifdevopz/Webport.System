using Blazor.WebportSystem.Common.Authentication;
using Blazor.WebportSystem.Common.Helpers;
using Blazor.WebportSystem.Common.HttpClients;
using Blazor.WebportSystem.Common.Services.Implementations;
using Blazor.WebportSystem.Common.Services.Interfaces;
using Blazor.WebportSystem.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Mudblazor
builder.Services.AddMudServices();

// Local Storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<LocalStorageService>();

// Http Client
builder.Services.AddHttpClient<BaseHttpClient>((sp, client) =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["BaseUrls:Production"]!);
});

builder.Services.AddHttpClient<TenantHttpClient>((sp, client) =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["Tenant:BaseUrl"]!);
});

builder.Services.AddScoped<BaseHttpClient>();
builder.Services.AddScoped<TenantHttpClient>();

// Services
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Authentication
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
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
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Blazor.WebportSystem.Client._Imports).Assembly)
    .AllowAnonymous();

await app.RunAsync();
