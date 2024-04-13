using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using ResaleRarities.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;
using Infrastructure.Services;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Radzen;
using Mopups.Hosting;
using Prism;
using OpenAI_API;
using OpenAI_API.Completions;
using ResaleRarities.Pages.Services;
using Microsoft.Extensions.Configuration;

namespace ResaleRarities
{
    public class MauiProgram
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
                    })
                   .ConfigureMopups();

                IConfigurationRoot config = new ConfigurationBuilder()
                   .AddUserSecrets<MauiProgram>()
                   .Build();

                builder.Services.AddSingleton(config);

                builder.Services.AddMauiBlazorWebView();

                // Read the connection string directly from appsettings.json
                string? connectionString = config["ConnectionStrings:Default"];

                if (connectionString == null)
                {
                    throw new Exception("Connection String not found");
                }
                //// Register the connection string with the App instance
                //builder.Services.AddSingleton(connectionString);

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure(maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null)));

                builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
                builder.Services.AddAuthorizationCore();
                builder.Services.AddHttpClient();
                builder.Services.AddMauiBlazorWebView();
                builder.Services.AddScoped<AutomatedOfferService>();
                builder.Services.AddScoped<IUnitofWork, UnitofWork>();
                builder.Services.AddScoped<Infrastructure.Services.NotificationService, Infrastructure.Services.NotificationService>();
                builder.Services.AddRadzenComponents();


#if DEBUG
                builder.Services.AddBlazorWebViewDeveloperTools();
                builder.Logging.AddDebug();
#endif
                builder.Services
                    .AddBlazorise(options =>
                    {
                        options.Immediate = true;
                    })
                    .AddBootstrapProviders()
                    .AddFontAwesomeIcons();

                var serviceProvider = builder.Services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedData.EnsurePopulated(dbContext);
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
