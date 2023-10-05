using Car_Rental;
using Car_Rental.Business.Classes;
using Car_Rental.Data.Classes;
using Car_Rental.Data.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<HttpClient>();

builder.Services.AddSingleton<BookingProcessor>();
builder.Services.AddSingleton<DataManagement>();
builder.Services.AddSingleton<AddCustomer>();
builder.Services.AddSingleton<AddVehicle>();
builder.Services.AddSingleton<IData, DataCollection>();
builder.Services.AddSingleton<IFetchData, FetchDataJson>();

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
