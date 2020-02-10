using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace POCRabbitMQ
{
    public class RabbitMQOptions
    {
        public CredentialsOptions Credentials { get; set;} = new CredentialsOptions();

        public EndpointOptions Endpoint { get; set; } = new EndpointOptions();

        //public Uri ManagementUrl { get; set; } = new Uri("http://localhost:15672");

        public Uri ManagementUrl { get; set; }

        public string VirtualHost { get; set; } = ConnectionFactory.DefaultVHost;

        public SettingsOptions Settings { get; set; } = new SettingsOptions();

        public IDictionary<string, ExchangeOptions> Exchanges { get; set; } = new Dictionary<string, ExchangeOptions>();
    }
}
