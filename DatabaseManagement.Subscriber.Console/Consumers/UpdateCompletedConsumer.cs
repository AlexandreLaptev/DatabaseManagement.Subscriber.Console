using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using DataManagement.MessageContracts;

namespace DatabaseManagement.Subscriber.Console
{
    public class UpdateCompletedConsumer : IConsumer<UpdateCompleted>
    {
        private readonly ILogger<UpdateCompletedConsumer> _logger;

        public UpdateCompletedConsumer(ILogger<UpdateCompletedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UpdateCompleted> context)
        {
            var consumerName = nameof(UpdateCompletedConsumer);
            _logger.LogInformation("Database updates have been completed. Received by {@consumerName}", consumerName);

            return Task.CompletedTask;
        }
    }
}