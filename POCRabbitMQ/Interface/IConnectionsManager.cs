using POCRabbitMQ;
using System;
using System.Threading.Tasks;

namespace POCRabbitMQ
{
    public interface IConnectionsManager : IDisposable
    {
        bool IsConnected { get; }

        bool IsConnecting { get; }

        Task SendAsync<T>(T obj, string exchangeName, string routingKey) where T : class;

        Task SendTextAsync(string message, string exchangeName, string routingKey);

        Task<IRabbitChannel> GetOrEstablishChannelAsync();
    }
}