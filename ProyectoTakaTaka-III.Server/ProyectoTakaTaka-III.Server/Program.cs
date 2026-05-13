using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka_III.BD.Datos;
using ProyectoTakaTaka_III.BD.Datos.Entity;
using ProyectoTakaTaka_III.Repositorio.Repositorios;
using ProyectoTakaTaka_III.Server.Client.Pages;
using ProyectoTakaTaka_III.Server.Components;
using ProyectoTakaTaka_III.Server.Components.Account;
using ProyectoTakaTaka_III.Servicio.ServicioHttp;
using ProyectoTakaTaka_III.Shared.Configuraciones;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("https://localhost:7073") });

builder.Services.AddScoped<IHttpServicio, HttpServicio>();

// Agregar controladores (API)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddSwaggerGen();



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
//Aca debajo de agrego un AddAuthorization para poder hacer el admin para que solo el admin pueda acceder a ciertas paginas
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
       policy.RequireAssertion(context =>
           context.User.Identity != null &&
           context.User.Identity.IsAuthenticated &&
           context.User.Identity.Name == "admintktk@gmail.com"));
});

builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<MiDbContext>(options =>
    options.UseSqlServer(connectionString));


// Registrar repositorios
builder.Services.AddScoped<IRepositorio<Evento>, Repositorio<Evento>>();
builder.Services.AddScoped<IRepositorioEvento, RepositorioEvento>();
builder.Services.AddScoped<IRepositorioCliente, RepositorioCliente>();
builder.Services.AddScoped<IRepositorioCombo, RepositorioCombo>();
builder.Services.AddScoped<IRepositorioOpcional, RepositorioOpcional>();
builder.Services.AddScoped<IRepositorioCumpleanero, RepositorioCumpleanero>();
builder.Services.AddScoped<IRepositorioHorario, RepositorioHorario>();
builder.Services.AddScoped<IRepositorioPago, RepositorioPago>();


builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<MiDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
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
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ProyectoTakaTaka_III.Server.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapControllers();

app.Run();
