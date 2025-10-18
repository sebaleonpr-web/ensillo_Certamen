using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models; // 👈 verifica que este namespace coincida con el que salga en tu carpeta Models

var builder = WebApplication.CreateBuilder(args);

// Lee la cadena de conexión del appsettings.json
var cs = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra tu contexto con SQL Server (usa tu base 'boletinLayon')
builder.Services.AddDbContext<boletinLayonContext>(opt => 
    opt.UseSqlServer(cs)
);

// Agrega MVC (ojo con la A mayúscula)
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

// Ruta por defecto (HomeController -> Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
