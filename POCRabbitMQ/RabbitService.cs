using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using POCRabbitMQ.ModelConfiguration;

namespace POCRabbitMQ
{
    class RabbitService
    {
        //private readonly IConnectionsManager connectionsManager;
        //private readonly RabbitMQOptions options;
        //private readonly ICollection<MessageTypeRecord> messageTypeRecords;

        //private TaskCompletionSource<bool> TaskCompletionSource { get; } = new TaskCompletionSource<bool>();
        //public Task<bool> ReadyTask => TaskCompletionSource.Task;

        ////internal IList<AsyncMediatingConsumer> Consumers { get; } = new List<AsyncMediatingConsumer>();

        ////private readonly IServiceScopeFactory scopeFactory;
        //private IRabbitChannel channel;
        //public RabbitService(IConnectionsManager connectionsManager,
        //                                     IOptions<RabbitMQOptions> options,
        //                                     //IServiceScopeFactory scopeFactory,
        //                                     IEnumerable<MessageTypeRecord> messageTypeRecords)
        //{
        //    this.connectionsManager = connectionsManager;
        //    //this.scopeFactory = scopeFactory;
        //    this.options = options.Value;
        //    this.messageTypeRecords = messageTypeRecords.ToList();
        //}

        //protected async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    await Task.Delay(options.Settings.MessageHandlerStartupDelay, stoppingToken);

        //    channel = await connectionsManager.GetOrEstablishChannelAsync();

        //    var queues = options.Exchanges.Values.SelectMany(exchange => exchange.Queues);

        //    await RegisterConsumersForAllQueues(queues);

        //    // wait for consumer to be ready.
        //    for (var timeout = 20; !AllConsumersAreRunning(Consumers) && timeout > 0; timeout--)
        //    {
        //        await Task.Delay(50);
        //    }

        //    TaskCompletionSource.SetResult(AllConsumersAreRunning(Consumers));
        //}

        //private static bool AllConsumersAreRunning(IEnumerable<AsyncMediatingConsumer> consumers)
        //{
        //    return consumers.All(consumer => consumer.IsRunning);
        //}

        //private async Task RegisterConsumersForAllQueues(IEnumerable<KeyValuePair<string, QueueOptions>> queues)
        //{
        //    foreach (var queue in queues)
        //    {
        //        await RegisterConsumersForQueue(queue.Key, queue.Value);
        //    }
        //}

        //private async Task<string> RegisterConsumersForQueue(string queueId, QueueOptions queueOptions)
        //{
        //    IBasicConsumer consumer;
        //    var records = messageTypeRecords
        //        .Where(record => record.QueueNameOrId == queueId || record.QueueNameOrId == queueOptions.Name)
        //        .ToList();

        //    if (!records.Any())
        //    {
        //        return null;
        //    }

        //    //var consumer = new AsyncMediatingConsumer(channel, scopeFactory, queueOptions.Name, connectionsManager, records);
        //    //Consumers.Add(consumer);

        //    return await channel.AddConsumer(queueOptions.Name, queueOptions.Name + "_consumer", consumer);
        //}



    }
}
