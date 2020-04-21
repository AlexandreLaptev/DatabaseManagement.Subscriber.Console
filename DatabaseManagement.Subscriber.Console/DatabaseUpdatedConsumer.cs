using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using DataManagement.MessageContracts;

namespace DatabaseManagement.Subscriber.Console
{
    public class DatabaseUpdatedConsumer : IConsumer<DatabaseUpdated>
    {
        private readonly ILogger<DatabaseUpdatedConsumer> _logger;

        public DatabaseUpdatedConsumer(ILogger<DatabaseUpdatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<DatabaseUpdated> context)
        {
            var consumerName = nameof(DatabaseUpdatedConsumer);
            _logger.LogInformation("Database has been updated. Received by {@consumerName}", consumerName);

            return Task.CompletedTask;
        }
    }
}