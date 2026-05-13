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
    //Entonces pongo este AddPolicy que se llama AdminOnly,
    //y le digo que para acceder a esa politica el usuario tiene que tener el rol de Admin

    options.AddPolicy("AdminOnly", policy =>
       policy.RequireRole("Admin"));

    /* Esto es para hacer el admin con el email, pero no es un rol, es una politica que se fija 
     * en el email del usuario, si el email es el del admin entonces puede acceder a las paginas restringidas, sino no puede acceder
    options.AddPolicy("AdminOnly", policy =>
       policy.RequireAssertion(context =>
           context.User.Identity != null &&
           context.User.Identity.IsAuthenticated &&
           context.User.Identity.Name == "admintktk@gmail.com"));*/
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
    //agrego el AddRoles para poder usar roles en la aplicacion, sino no se pueden usar roles y no se puede hacer el admin
    .AddRoles<IdentityRole>()
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

// Aca hago esto para crear el rol de admin y asignarlo al usuario admin,
// esto se hace para que el usuario admin pueda acceder a las paginas restringidas solo para admin
using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Se cre el rol del admin 
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Busca el usuario por su email
    var usuario =
        await userManager.FindByEmailAsync("admintktk@gmail.com");

    // Se asigna el rol de admin al usuario, pero solo si el usuario existe y no tiene el rol de admin asignado
    if (usuario != null &&
        !await userManager.IsInRoleAsync(usuario, "Admin"))
    {
        await userManager.AddToRoleAsync(usuario, "Admin");
    }
}

app.Run();
