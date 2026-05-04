using RecetaApp;
using RecetaApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<RecetaService>();
builder.Services.AddScoped<MedicamentoService>();
builder.Services.AddScoped<CatalogoMedicamentoService>();
builder.Services.AddScoped<PresionArterialService>();

await builder.Build().RunAsync();
