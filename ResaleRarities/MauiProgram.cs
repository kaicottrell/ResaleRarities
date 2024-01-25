using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using ResaleRarities.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ResaleRarities
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            try
            {
                var builder = MauiApp.CreateBuilder();
                builder
                    .UseMauiApp<App>()
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    });

                builder.Services.AddMauiBlazorWebView();

                // Read the connection string directly from appsettings.json
                string connectionString = "Data Source=resalerarity.database.windows.net,1433;Initial Catalog=ResaleRarities;User Id=kcadmin;Password=rrforweber7!;Encrypt=True;";

                // Register the connection string with the App instance
                builder.Services.AddSingleton(connectionString);

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure(maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null)));

                builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
                builder.Services.AddAuthorizationCore();
                builder.Services.AddHttpClient();
                builder.Services.AddMauiBlazorWebView();
                builder.Services.AddScoped<IUnitofWork, UnitofWork>();
#if DEBUG
                builder.Services.AddBlazorWebViewDeveloperTools();
                builder.Logging.AddDebug();
#endif

                return builder.Build();
            }
            catch (Exception ex)
            {
                // Log the error to the console
                Console.WriteLine($"An error occurred while creating the Maui app: {ex}");
                // Re-throw the exception if needed
                throw;
            }
        }
    }
}
