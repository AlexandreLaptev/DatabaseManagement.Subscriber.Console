using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using DataManagement.MessageContracts;

namespace DatabaseManagement.Subscriber.Console
{
    public class UpdateProgressConsumer : IConsumer<UpdateProgress>
    {
        private readonly ILogger<UpdateProgressConsumer> _logger;

        public UpdateProgressConsumer(ILogger<UpdateProgressConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UpdateProgress> context)
        {
            var message = context.Message.Message;
            var status = context.Message.Status;
            var consumerName = nameof(UpdateProgressConsumer);
            _logger.LogInformation("Progress Message: {@message}, Status: {@status} received by {@consumerName}", message, status, consumerName);

            return Task.CompletedTask;
        }
    }
}