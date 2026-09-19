var graditelj = WebApplication.CreateBuilder(args);

graditelj.Services.AddHttpClient("default")

    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

graditelj.Services.AddControllersWithViews()
    .AddRazorOptions(opcije =>
    {
        opcije.ViewLocationFormats.Clear();
        opcije.ViewLocationFormats.Add(
            "/KorisnickiInterfejs/Views/{1}/{0}.cshtml");
        opcije.ViewLocationFormats.Add(
            "/KorisnickiInterfejs/Views/Shared/{0}.cshtml");
    });

graditelj.Services.AddDistributedMemoryCache();
graditelj.Services.AddSession(opcije =>
{
    opcije.IdleTimeout = TimeSpan.FromMinutes(30);
    opcije.Cookie.HttpOnly = true;
    opcije.Cookie.IsEssential = true;
});

graditelj.Services.AddHttpClient();

var aplikacija = graditelj.Build();
aplikacija.UseHttpsRedirection();
aplikacija.UseStaticFiles();
aplikacija.UseRouting();
aplikacija.UseSession();
aplikacija.UseAuthorization();

aplikacija.MapControllerRoute(
    name: "podrazumevana",
    pattern: "{controller=Nalog}/{action=Prijava}/{id?}");

aplikacija.Run();