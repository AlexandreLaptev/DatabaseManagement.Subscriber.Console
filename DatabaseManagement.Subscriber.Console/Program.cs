using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using DataManagement.MessageContracts;

namespace DatabaseManagement.Subscriber.Console
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var builder = new HostBuilder()
                    .UseSerilog()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddMassTransit(config =>
                        {
                            config.AddConsumer<UpdateProgressConsumer>();
                            config.AddConsumer<UpdateCompletedConsumer>();
                            config.AddBus(ConfigureBus);
                        });

                        services.AddSingleton<IHostedService, MassTransitConsoleHostedService>();
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddSerilog();
                    });

                await builder.RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static IBusControl ConfigureBus(IServiceProvider provider)
        {
            var rabbitMQBus = Bus.Factory.CreateUsingRabbitMq(busFactoryConfig =>
            {
                busFactoryConfig.Message<UpdateProgress>(configTopology => configTopology.SetEntityName("database.updates.progress"));
                busFactoryConfig.Message<UpdateCompleted>(configTopology => configTopology.SetEntityName("database.updates.completed"));

                var host = busFactoryConfig.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Setup RabbitMQ queue consumer
                busFactoryConfig.ReceiveEndpoint("database-update-subscribers", configurator =>
                {
                    configurator.PrefetchCount = 1;
                    configurator.Consumer<UpdateProgressConsumer>(provider);
                    configurator.Consumer<UpdateCompletedConsumer>(provider);
                });
            });

            return rabbitMQBus;
        }
    }
}