using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POCRabbitMQ
{
    public interface IRabbitChannel : IDisposable
    {
        IModel Model { get; }

        Task SendAsync<T>(T obj, string exchangeName, string routingKey) where T : class;

        Task SendTextAsync(string message, string exchangeName, string routingKey, bool isJson);

        Task SendBytesAsync(byte[] bytes, string exchangeName, string routingKey, bool isJson);

        Task<string> AddConsumer(string queue, string consumerTag, IBasicConsumer consumer);
    }
}
