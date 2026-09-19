using Microsoft.EntityFrameworkCore;
using SlojPodataka.TehnoloskeKlase;

var graditelj = WebApplication.CreateBuilder(args);

var nizKonekcije = graditelj.Configuration
    .GetConnectionString("EvidencijaZadatakaDB")
    ?? Konekcija.NizKonekcije;

graditelj.Services.AddDbContext<EvidencijaDbContext>(opcije =>
    opcije.UseSqlServer(nizKonekcije));

graditelj.Services.AddScoped<KorisnikRepozitorijum>();
graditelj.Services.AddScoped<ZadatakRepozitorijum>();

graditelj.Services.AddControllers()
    .AddJsonOptions(opcije =>
    {
        opcije.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });


graditelj.Services.AddCors(o => o.AddPolicy("DozvoliSve", b =>
    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var aplikacija = graditelj.Build();

aplikacija.UseCors("DozvoliSve");
aplikacija.UseAuthorization();
aplikacija.MapControllers();
using (var opseg = aplikacija.Services.CreateScope())
{
    var kontekst = opseg.ServiceProvider.GetRequiredService<EvidencijaDbContext>();
    var putanjaXml = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "..", "..", "..", "..",
        "SlojPodataka", "XML", "pocetni_podaci.xml");
    putanjaXml = Path.GetFullPath(putanjaXml);
    Console.WriteLine($"Putanja XML: {putanjaXml}");
    Console.WriteLine($"Fajl postoji: {File.Exists(putanjaXml)}");
    PocetniPodaci.PopuniSve(kontekst, putanjaXml);

}
aplikacija.Run();