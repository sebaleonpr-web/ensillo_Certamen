using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using enso_Certamen.Data;

var builder = WebApplication.CreateBuilder(args);

// Lee la cadena de conexión del appsettings.json
var cs = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra tu contexto con SQL Server (usa tu base 'boletinLayon')
builder.Services.AddDbContext<BoletinLayonContext>(opt =>
    opt.UseSqlServer(cs)
);

// Agrega MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuración para manejo de errores y HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();


// ---------- 🌎 Configurar idioma global español (Chile) ----------
var cultureInfo = new CultureInfo("es-CL");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = new List<CultureInfo> { cultureInfo },
    SupportedUICultures = new List<CultureInfo> { cultureInfo }
};

app.UseRequestLocalization(localizationOptions);
// ---------------------------------------------------------------


// Ruta por defecto (HomeController -> Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
