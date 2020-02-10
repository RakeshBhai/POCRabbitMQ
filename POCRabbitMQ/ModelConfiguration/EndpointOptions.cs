using RabbitMQ.Client;
using System;

namespace POCRabbitMQ
{
    public class EndpointOptions
    {
        //public string Hostname { get; set; } = "localhost";
        public string Hostname { get; set; }
        public int Port { get; set; } = AmqpTcpEndpoint.UseDefaultPort;
        public string SslCertPath { get; set; } = "";
        public string SslCertPassphrase { get; set; } = "";
        public bool SslEnabled { get; set; } = false;
    }
}
