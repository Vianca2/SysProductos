using Proyecto.DAL;
using Microsoft.EntityFrameworkCore;
using SysInventory.DAL;

var builder = WebApplication.CreateBuilder(args);

// Agregar Entity Framework y la conexión a la BD
builder.Services.AddDbContext<InventoryDBContext>(options =>
{
    var conexionString = builder.Configuration.GetConnectionString("Conn");

    options.UseMySql(conexionString, ServerVersion.AutoDetect(conexionString));

});

   

// Agregar soporte para controladores y vistas MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el pipeline de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configurar la ruta predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();


 
