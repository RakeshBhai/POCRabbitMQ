using System;
//using Eom.RabbitMQ.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics;
using POCRabbitMQ;
//using POCRabbitMQ.ConnectionsManager;

namespace POCRabbitMQ
{
    public class ConnectionsManager : IConnectionsManager
    {
        private readonly RabbitMQOptions options;
        //private readonly ILogger logger;

        private IConnection connection;
        private IRabbitChannel channel;

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public bool IsConnected => channel != null;

        public bool IsConnecting { get; private set; }

        //private CountdownTimer retryTimer;

        public ConnectionsManager(
            IOptions<RabbitMQOptions> options)
        {
            this.options = options.Value;
            //this.logger = logger;

            //retryTimer = new CountdownTimer();
        }

        public async Task SendAsync<T>(T obj, string exchangeName, string routingKey) where T : class
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (exchangeName is null)
            {
                throw new ArgumentNullException(nameof(exchangeName));
            }

            if (routingKey is null)
            {
                throw new ArgumentNullException(nameof(routingKey));
            }

            var channel = await GetOrEstablishChannelAsync();
            await channel.SendAsync(obj, exchangeName, routingKey);
        }

        public async Task SendTextAsync(string message, string exchangeName, string routingKey)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (exchangeName is null)
            {
                throw new ArgumentNullException(nameof(exchangeName));
            }

            if (routingKey is null)
            {
                throw new ArgumentNullException(nameof(routingKey));
            }

            var channel = await GetOrEstablishChannelAsync();
            await channel.SendTextAsync(message, exchangeName, routingKey, true);
        }

        public async Task<IRabbitChannel> GetOrEstablishChannelAsync()
        {
            if (channel != null)
            {
                return channel;
            }

            await semaphore.WaitAsync();

            IsConnecting = true;

            // check again as another task may have established the channel while we awaited the semaphore
            if (channel != null)
            {
                IsConnecting = false;
                return channel;
            }

            try
            {
                connection = RabbitSetupSetting.GetConnection(options);
                connection.CallbackException += ConnectionCallbackExceptionHandler;
                connection.ConnectionRecoveryError += ConnectionRecoveryErrorHandler;

                var model = RabbitSetupSetting.GetChannel(connection);
                channel = new RabbitChannel(model, options.Endpoint.Hostname);

                RabbitSetupSetting.StartExchanges(model, options.Exchanges.Values);

                return channel;
            }
            catch (BrokerUnreachableException)
            {
                //retryTimer.Reset(TimeSpan.FromSeconds(5));
                return null;
            }
            finally
            {
                IsConnecting = false;
                semaphore.Release();
            }
        }

        private void ConnectionCallbackExceptionHandler(object sender, CallbackExceptionEventArgs eventArgs)
        {
            if (eventArgs is null)
            {
                return;
            }

            //logger.LogError(new EventId(), eventArgs.Exception, eventArgs.Exception.Message, eventArgs);

            throw eventArgs.Exception;
        }

        private void ConnectionRecoveryErrorHandler(object sender, ConnectionRecoveryErrorEventArgs eventArgs)
        {
            if (eventArgs is null)
            {
                return;
            }

            //logger.LogError(new EventId(), eventArgs.Exception, eventArgs.Exception.Message, eventArgs);

            throw eventArgs.Exception;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.CallbackException -= ConnectionCallbackExceptionHandler;
                connection.ConnectionRecoveryError -= ConnectionRecoveryErrorHandler;
            }

            channel?.Dispose();

            if (connection?.IsOpen == true)
            {
                connection.Close();
            }
        }
    }
}
