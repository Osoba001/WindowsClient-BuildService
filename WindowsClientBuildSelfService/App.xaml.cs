using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsClientBuildSelfService.Main;
using WindowsClientBuildSelfService.PR.Models;
using WindowsClientBuildSelfService.PR.Services;

namespace WindowsClientBuildSelfService
{
    /// <summary>
    /// Represents the WPF application object, which is responsible for configuring and registering the application services and resources.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets or sets the service provider instance for the application.
        /// </summary>
        public static IServiceProvider serviceProvider { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class, creates an <see cref="IServiceCollection"/> object, registers the application services using the <see cref="RegisteService"/> method, and builds the <see cref="IServiceProvider"/> instance.
        /// </summary>
        public App()
        {
            ServiceCollection service = new();
            RegisteService(service);
            serviceProvider = service.BuildServiceProvider();

        }

        /// <summary>
        /// Overrides the <see cref="Application.OnStartup"/> method to show the startup window view when the application starts up.
        /// </summary>
        /// <param name="e">A <see cref="StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            serviceProvider.GetRequiredService<StartupWindowsView>().Show(); ;
        }

        /// <summary>
        /// Registers the application services with the specified <see cref="IServiceCollection"/> object.
        /// </summary>
        /// <param name="service">The <see cref="IServiceCollection"/> object to register the services with.</param>
        private static void RegisteService(IServiceCollection service)
        {
            service.AddSingleton<StartupWindowsView>();
            service.AddScoped<StartupWindowsViewModel>();
            service.AddScoped<GitConfigData>();
            GitConfigData config = new();
            service.AddHttpClient<IPullRequestService, PullRequestService>(option =>
            {
                option.BaseAddress = new Uri($"{config.RepoBaseUri}{config.Owner}/");
                option.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                option.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.AccessToken);
                option.DefaultRequestHeaders.Add("X-GitHub-Api-Version", config.ApiVersion);
                option.DefaultRequestHeaders.Add("User-Agent", "SEPAL-Builds-Self-Service");
            });
        }

        private static string protect(string unprotectData = "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")
        {
            byte[] entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
            byte[] data = Encoding.ASCII.GetBytes(unprotectData);
           return Convert.ToBase64String(ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser));
        }
    }
}
