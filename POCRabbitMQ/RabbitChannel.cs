using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using RabbitMQ.Client.Events;
//using Microsoft.Extensions.Options;
using System.Net;
using POCRabbitMQ;

namespace POCRabbitMQ
{

    internal class RabbitChannel : IRabbitChannel
    {
        //private readonly ILogger logger;
        private readonly string hostname;

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public IModel Model { get; }

        public RabbitChannel(IModel channel,string hostname)
        {
            Model = channel;
            //this.logger = logger;
            this.hostname = hostname;

            Model.CallbackException += ChannelCallbackExceptionHandler;
            Model.BasicRecoverOk += ChannelBasicRecoverOkHandler;
        }

        public async Task SendAsync<T>(T obj, string exchangeName, string routingKey) where T : class
        {
            var json = JsonConvert.SerializeObject(obj);

            await SendTextAsync(json, exchangeName, routingKey, isJson: true);
        }

        public async Task SendTextAsync(string message, string exchangeName, string routingKey, bool isJson)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            await SendBytesAsync(bytes, exchangeName, routingKey, isJson);
        }

        public async Task SendBytesAsync(byte[] bytes, string exchangeName, string routingKey, bool isJson)
        {
            await semaphore.WaitAsync();

            try
            {
                Model.BasicPublish(exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: CreateProperties(Model, isJson),
                    body: bytes);
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<string> AddConsumer(string queue, string consumerTag, IBasicConsumer consumer)
        {
            await semaphore.WaitAsync();

            try
            {
                bool autoAck = false;
                bool noLocal = false;
                bool exclusive = false;
                IDictionary<String, Object> arguments = null;

                var sanitizedArguments = RabbitSetupSetting.GetSanitizedArguments(arguments);
                return Model.BasicConsume(queue, autoAck, consumerTag, noLocal, exclusive, sanitizedArguments, consumer);
            }
            finally
            {
                semaphore.Release();
            }
        }


        private static IBasicProperties CreateProperties(IModel channel, bool isJson)
        {
            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            if (isJson)
            {
                properties.ContentType = "application/json";
            }

            return properties;
        }

        private void ChannelBasicRecoverOkHandler(object sender, EventArgs eventArgs)
        {
            if (eventArgs is null)
            {
                return;
            }

            //logger.LogInformation($"RabbitMQ connection to {hostname} has been reestablished.");
        }

        private void ChannelCallbackExceptionHandler(object sender, CallbackExceptionEventArgs eventArgs)
        {
            if (eventArgs is null)
            {
                return;
            }

            //logger.LogError(new EventId(), eventArgs.Exception, eventArgs.Exception.Message, eventArgs);
        }

        public void Dispose()
        {

            if (Model != null)
            {
                Model.CallbackException -= ChannelCallbackExceptionHandler;
                Model.BasicRecoverOk -= ChannelBasicRecoverOkHandler;
            }

            if (Model?.IsOpen == true)
            {
                Model.Close((int)HttpStatusCode.OK, "Channel closed");
            }


        }
    }
}