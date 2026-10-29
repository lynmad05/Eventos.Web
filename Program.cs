using System.Globalization;
using Eventos.Web.Data;
using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

// Cultura peruana (punto decimal)
var cultura = new CultureInfo("es-PE");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EventosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventosDB")));

builder.Services.AddScoped<EventoRepositorio>();
builder.Services.AddScoped<AsistenteRepositorio>();
builder.Services.AddScoped<InscripcionRepositorio>();

var app = builder.Build();

// Datos iniciales (después de aplicar la migración)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventosContext>();

    if (!await context.Categorias.AnyAsync())
    {
        context.Categorias.AddRange(
            new Categoria { Nombre = "Tecnología" },
            new Categoria { Nombre = "Cultura" },
            new Categoria { Nombre = "Deportes" });
        await context.SaveChangesAsync();
    }

    if (!await context.Eventos.AnyAsync())
    {
        var categorias = await context.Categorias.ToListAsync();
        int Cat(string nombre) => categorias.First(c => c.Nombre == nombre).Id;

        context.Eventos.AddRange(
            new Evento { Titulo = "Conferencia de Inteligencia Artificial", Fecha = DateTime.Today.AddDays(10).AddHours(18), Lugar = "Auditorio TECSUP", Aforo = 150, Precio = 50.00m, CategoriaId = Cat("Tecnología") },
            new Evento { Titulo = "Taller de Desarrollo Web", Fecha = DateTime.Today.AddDays(15).AddHours(9), Lugar = "Laboratorio 3", Aforo = 30, Precio = 0m, CategoriaId = Cat("Tecnología") },
            new Evento { Titulo = "Festival de Música Andina", Fecha = DateTime.Today.AddDays(20).AddHours(19), Lugar = "Parque de la Exposición", Aforo = 500, Precio = 35.50m, CategoriaId = Cat("Cultura") },
            new Evento { Titulo = "Maratón 10K Lima", Fecha = DateTime.Today.AddDays(30).AddHours(7), Lugar = "Costa Verde", Aforo = 1000, Precio = 25.00m, CategoriaId = Cat("Deportes") });
        await context.SaveChangesAsync();
    }

    if (!await context.Asistentes.AnyAsync())
    {
        context.Asistentes.AddRange(
            new Asistente { Nombres = "Ailyn Torres", Dni = "70123456", Correo = "ailyn.torres@mail.com" },
            new Asistente { Nombres = "Carlos Ramos", Dni = "71234567", Correo = "carlos.ramos@mail.com" },
            new Asistente { Nombres = "Lucía Fernández", Dni = "72345678", Correo = "lucia.fernandez@mail.com" },
            new Asistente { Nombres = "Miguel Quispe", Dni = "73456789", Correo = "miguel.quispe@mail.com" },
            new Asistente { Nombres = "Valeria Soto", Dni = "74567890", Correo = "valeria.soto@mail.com" });
        await context.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new List<CultureInfo> { cultura },
    SupportedUICultures = new List<CultureInfo> { cultura }
});

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Eventos}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();