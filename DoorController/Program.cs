using DoorController.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Leer la cadena de conexión: ENV (prioridad) o appsettings.json
var envConn = Environment.GetEnvironmentVariable("ConnectionStrings__Default");
var cfgConn = builder.Configuration.GetConnectionString("Default");
var connStr = !string.IsNullOrWhiteSpace(envConn) ? envConn : cfgConn;

if (string.IsNullOrWhiteSpace(connStr))
{
    throw new InvalidOperationException(
        "No hay cadena de conexión. Define 'ConnectionStrings__Default' (ENV) o appsettings.json -> ConnectionStrings:Default");
}

// 2) Log seguro de la cadena (oculta la contraseña)
string Mask(string s)
{
    if (string.IsNullOrWhiteSpace(s)) return s ?? "";
    var idx = s.IndexOf("Password=", StringComparison.OrdinalIgnoreCase);
    if (idx < 0) return s;
    var end = s.IndexOf(';', idx);
    if (end < 0) end = s.Length;
    return s.Remove(idx, end - idx).Insert(idx, "Password=****");
}
Console.WriteLine($"[DoorController] ConnectionString utilizada: {Mask(connStr)}");

// 3) Registrar DbContext según sea Postgres (Render) o SQLite (local)
builder.Services.AddDbContext<DoorDbContext>(opt =>
{
    if (connStr.Contains("Host=", StringComparison.OrdinalIgnoreCase))
    {
        // Postgres (Render)
        opt.UseNpgsql(connStr, npg => npg.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
    }
    else
    {
        // SQLite (local)
        opt.UseSqlite(connStr);
    }
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// 4) Crear BD + seed, con manejo amable de errores
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DoorDbContext>();
    try
    {
        db.Database.EnsureCreated();

        if (!db.Doors.Any())
        {
            db.Doors.AddRange(new[] {
                new DoorController.Models.Door { Name = "Puerta de Jardín" },
                new DoorController.Models.Door { Name = "Puerta Delantera" },
                new DoorController.Models.Door { Name = "Puerta de Garaje" },
                new DoorController.Models.Door { Name = "Puerta Cuarto 1" },
                new DoorController.Models.Door { Name = "Puerta Cuarto 2" },
            });
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("[DoorController] Error inicializando la BD:");
        Console.WriteLine(ex.Message);
        Console.WriteLine("Sugerencias:");
        Console.WriteLine(" - Si estás probando local, elimina/limpia la variable de entorno ConnectionStrings__Default para usar SQLite.");
        Console.WriteLine(" - Si usas Render, verifica que el Host= sea EXACTO y haya conectividad a puerto 5432.");
        throw; // re-lanza para ver la traza en VS si quieres
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
