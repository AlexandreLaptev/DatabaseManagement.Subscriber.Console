using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using DataManagement.MessageContracts;

namespace DatabaseManagement.Subscriber.Console
{
    public class ProgressMessageConsumer : IConsumer<ProgressMessage>
    {
        private readonly ILogger<ProgressMessageConsumer> _logger;

        public ProgressMessageConsumer(ILogger<ProgressMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ProgressMessage> context)
        {
            var message = context.Message.Message;
            var status = context.Message.Status;
            var consumerName = nameof(ProgressMessageConsumer);
            _logger.LogInformation("Progress - Message: {@message}, Status: {@status} received by {@consumerName}", message, status, consumerName);

            return Task.CompletedTask;
        }
    }
}