using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProyectoTakaTaka_III.Servicio.ServicioHttp;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.Identity != null &&
            context.User.Identity.IsAuthenticated &&
            context.User.Identity.Name == "admintktk@gmail.com"));
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IHttpServicio, HttpServicio>();

await builder.Build().RunAsync();
