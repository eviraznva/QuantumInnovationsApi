using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using QuantumInnovationsApi.Models;
using Microsoft.EntityFrameworkCore;
using QuantumInnovationsApi.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<QuantumInnovationsDb>();

var app = builder.Build();

{
    using var scope =  app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<QuantumInnovationsDb>();
    if (context.Database.EnsureCreated() || !context.Raports.Any())
    {
        
        context.Raports.Add(new Raport
        {
            Title = "Raport 1: Infrastruktura sieciowa",
            Date = DateOnly.Parse("2021-01-01"),
            Content = "W raporcie przedstawiono obecną konfigurację infrastruktury sieciowej Quantum Innovations. Sieć składa się z serwera głównego (Ubuntu Server 24.04 LTS) oraz 30 stacji klienckich (Linux Mint). Serwer zarządza usługami takimi jak DNS, DHCP, HTTP/S, MySQL, PHP, phpMyAdmin, WordPress, RAID, SAMBA, Squid, Postfix oraz Dovecot. Zastosowano macierz RAID 5, która zapewnia odpowiedni poziom redundancji danych.",
            Author = "Jan Kowalski",
            Conclusions = "Infrastruktura sieciowa jest dobrze zaprojektowana i zrealizowana, jednak zaleca się regularne monitorowanie obciążenia serwera oraz aktualizację oprogramowania w celu zachowania bezpieczeństwa.",
            Details = ["Adres sieci wewnętrznej: 192.168.230.0/24", "Brama sieci wewnętrznej: 192.168.230.1", "Serwer DNS: bind9", "Serwer DHCP: ISC KEA", "Serwer HTTP/S: Apache", "Serwer pocztowy: Postfix i Dovecot"]
        });

        context.Raports.Add(new Raport
        {
            Title = "Raport 2: Bezpieczeństwo sieciowe",
            Date = DateOnly.Parse("2021-02-01"),
            Content = "W raporcie przedstawiono analizę bezpieczeństwa sieciowego Quantum Innovations. Sieć jest zabezpieczona przed atakami z zewnątrz za pomocą firewalla, który blokuje nieautoryzowany ruch sieciowy. Ponadto zainstalowano oprogramowanie antywirusowe na wszystkich stacjach klienckich oraz serwerze głównym. Wdrożono również system monitorowania logów w celu wykrywania podejrzanej aktywności.",
            Author = "Anna Nowak",
            Conclusions = "Bezpieczeń stwo sieciowe jest na odpowiednim poziomie, jednak zaleca się regularne aktualizacje oprogramowania oraz przeprowadzanie audytów bezpieczeństwa w celu zapobiegania ewentualnym atakom.",
            Details = ["Zdalny dostęp SSH: OpenSSH z kluczami publicznymi", "Proxy: Squid z funkcją monitorowania ruchu", "Codzienne archiwizowanie systemu plików: crontab"]
        });

        context.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/raports", async (QuantumInnovationsDb context) =>
{
    var raports = await context.Raports.ToListAsync();
    
    return Results.Ok(raports);
})
.WithName("GetRaports")
.WithOpenApi();

app.MapGet("/raports/{id}", async (QuantumInnovationsDb context, int id) =>
{
    var raport = await context.Raports.FindAsync(id);

    return raport == null ? Results.NotFound() : Results.Ok(raport);
});

app.MapPost("/raports", async (QuantumInnovationsDb context, Raport raport) =>
{
    context.Raports.Add(raport);
    await context.SaveChangesAsync();

    return Results.Created($"/raports/{raport.Id}", raport);
});

app.MapPut("/raports/{id}", async (QuantumInnovationsDb context, int id, Raport raport) =>
{
    if (id != raport.Id)
    {
        return Results.BadRequest();
    }

    context.Entry(raport).State = EntityState.Modified;
    await context.SaveChangesAsync();

    return Results.Ok(raport);
});

app.MapDelete("/raports/{id}", async (QuantumInnovationsDb context, int id) =>
{
    var raport = await context.Raports.FindAsync(id);

    if (raport == null)
    {
        return Results.NotFound();
    }

    context.Raports.Remove(raport);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();