using System;
using System.Threading;
using Evo2HomeMqttNet.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEscapades.Extensions.Logging.RollingFile;
using Polly;

namespace Evo2HomeMqttNet
{
    internal class Program
    {
        private const string Evousername = "evoUsername";
        private const string Evopassword = "evoPassword";
        private const string Evolocationname = "evoLocationName";
        private const string Evopollduration = "evoPollDuration";
        private const string Mqttconnection = "mqttConnection";
        private const string Mqttpassword = "mqttPassword";
        private const string Mqttprefix = "mqttPrefix";
        private const string Mqttuser = "mqttUser";
        private const string Mqttclientname = "mqttClientName";
        private const string Mqttport = "mqttPort";
        private const string Disablemqtt = "disableMqtt";
        private const string Mqqtdiscovery = "mqqtDiscovery";
        private const string Mqqtdiscoveryprefix = "mqqtDiscoveryPrefix";
        private const string Evofilelocation = "evoFileLocation";
        private const string ApiLocation = "https://tccna.honeywell.com/WebAPI/";
        private const string HoneyWellRoot = "https://tccna.honeywell.com/";

        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var log = serviceProvider.GetService<ILogger<Program>>();
            var service = serviceProvider.GetService<EvoHomeWorker>();
            log.LogInformation("Starting Application");

            service.Start().GetAwaiter().GetResult();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var settings = GetSettings();

            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddFile(c =>
                {
                    c.LogDirectory = settings.FileLocation;
                    c.Periodicity = PeriodicityOptions.Daily;
                    c.RetainedFileCountLimit = 2;
                });
            });

            services.AddSingleton(settings);

            services.AddHttpClient(Constants.EvoClientName,
                    client => { client.BaseAddress = new Uri(ApiLocation); })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }));

            services.AddHttpClient(Constants.TokenServiceClientName,
                    client => { client.BaseAddress = new Uri(HoneyWellRoot); })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }));

            services.AddSingleton<Store>();
            services.AddSingleton<TokenManager>();
            services.AddSingleton<EvoHomeWorker>();
            services.AddSingleton<MqttWorker>();
        }

        private static EvoHomeSettings GetSettings()
        {
            return new EvoHomeSettings
            {
                EvoUsername = Environment.GetEnvironmentVariable(Evousername),
                EvoPassword = Environment.GetEnvironmentVariable(Evopassword),
                LocationName = Environment.GetEnvironmentVariable(Evolocationname),
                PollRate = TimeSpan.FromSeconds(
                    int.TryParse(Environment.GetEnvironmentVariable(Evopollduration), out var result)
                        ? result
                        : 180),
                MqttConnection = Environment.GetEnvironmentVariable(Mqttconnection),
                MqttPassword = Environment.GetEnvironmentVariable(Mqttpassword),
                MqttPrefix = Environment.GetEnvironmentVariable(Mqttprefix),
                MqttUser = Environment.GetEnvironmentVariable(Mqttuser),
                MqttClientName = Environment.GetEnvironmentVariable(Mqttclientname),
                MqttPort = int.TryParse(Environment.GetEnvironmentVariable(Mqttport), out var port)
                    ? port
                    : Constants.DefaultMqttPort,
                DisableMqtt = bool.TryParse(Environment.GetEnvironmentVariable(Disablemqtt), out var disableMqtt) &&
                              disableMqtt,
                MqqtDiscovery =
                    bool.TryParse(Environment.GetEnvironmentVariable(Mqqtdiscovery), out var mqqtDiscovery) &&
                    mqqtDiscovery,
                MqqtDiscoveryPrefix = Environment.GetEnvironmentVariable(Mqqtdiscoveryprefix),
                FileLocation = Environment.GetEnvironmentVariable(Evofilelocation)
            };
        }
    }
}