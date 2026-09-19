using Microsoft.EntityFrameworkCore;
using SlojPodataka.TehnoloskeKlase;

var graditelj = WebApplication.CreateBuilder(args);

var nizKonekcije = graditelj.Configuration
    .GetConnectionString("EvidencijaZadatakaDB")
    ?? Konekcija.NizKonekcije;

graditelj.Services.AddDbContext<EvidencijaDbContext>(opcije =>
    opcije.UseSqlServer(nizKonekcije));

graditelj.Services.AddScoped<KorisnikRepozitorijum>();

graditelj.Services.AddControllers();

graditelj.Services.AddCors(o => o.AddPolicy("DozvoliSve", b =>
    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var aplikacija = graditelj.Build();

aplikacija.UseCors("DozvoliSve");
aplikacija.UseAuthorization();
aplikacija.MapControllers();
aplikacija.Run();