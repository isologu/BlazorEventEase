using BlazorEventEase.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IEventStore, InMemoryEventStore>();
builder.Services.AddScoped<IUserSessionTracker, UserSessionTracker>();

await builder.Build().RunAsync();