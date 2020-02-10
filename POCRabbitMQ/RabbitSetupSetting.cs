using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace POCRabbitMQ
{
    class RabbitSetupSetting
    {
        public static IConnection GetConnection(RabbitMQOptions options)
        {
            //var sslOptions = new SslOption(options.Endpoint.Hostname,
                                           //options.Endpoint.SslCertPath,
                                           //options.Endpoint.SslEnabled);

            var factory = new ConnectionFactory
            {
                HostName = options.Endpoint.Hostname,
                Port = options.Endpoint.Port,
                //Ssl = sslOptions,
                UserName = options.Credentials.Username,
                Password = options.Credentials.Password,
                VirtualHost = options.VirtualHost,
                RequestedChannelMax = options.Settings.RequestedChannelMax,
                RequestedFrameMax = options.Settings.RequestedFrameMax,
                AutomaticRecoveryEnabled = options.Settings.AutomaticRecoveryEnabled,
                TopologyRecoveryEnabled = options.Settings.TopologyRecoveryEnabled,
                RequestedConnectionTimeout = options.Settings.RequestedConnectionTimeout,
                RequestedHeartbeat = options.Settings.RequestedHeartbeat,
                DispatchConsumersAsync = true
            };

            return factory.CreateConnection();
        }
        public static IModel GetChannel(IConnection connection)
        {
            return connection.CreateModel();
        }

        public static void StartExchanges(IModel channel, IEnumerable<ExchangeOptions> exchangeOptions)
        {
            //ExchangeOptions exchangeOptionsobj = new ExchangeOptions();
            //if (exchangeOptions.Count() == 0)
            //{

            //    exchangeOptionsobj.Name= "main_exchange";
            //    exchangeOptionsobj.Type = "direct";
            //    exchangeOptionsobj.Durable = true;
            //    exchangeOptionsobj.AutoDelete=false;
            //    exchangeOptionsobj.Arguments= new Dictionary<string, object>();


            //}

            foreach (var exchangeOption in exchangeOptions)
            {
                StartExchange(channel, exchangeOption);
            }
        }

        private static void StartExchange(IModel channel, ExchangeOptions exchangeOptions)
        {
            channel.ExchangeDeclare(exchangeOptions.Name,
                                    exchangeOptions.Type,
                                    exchangeOptions.Durable,
                                    exchangeOptions.AutoDelete,
                                    exchangeOptions.Arguments);

            StartQueues(channel, exchangeOptions.Name, exchangeOptions.Queues.Values);
        }

        private static void StartQueues(IModel channel, string exchangeName, IEnumerable<QueueOptions> queueOptions)
        {
            foreach (var queueOption in queueOptions)
            {
                StartQueue(channel, exchangeName, queueOption);
            }
        }

        private static void StartQueue(IModel channel, string exchangeName, QueueOptions queueOption)
        {
            channel.QueueDeclare(queueOption.Name,
                                 queueOption.Durable,
                                 queueOption.Exclusive,
                                 queueOption.AutoDelete,
                                 GetSanitizedArguments(queueOption.Arguments));

            IEnumerable<string> routingKeys = queueOption.RoutingKeys.DefaultIfEmpty(queueOption.Name);
            StartBindings(channel, queueOption.Name, exchangeName, routingKeys);
        }

        private static void StartBindings(IModel channel, string queueName, string exchangeName, IEnumerable<string> routingKeys)
        {
            foreach (var routingKey in routingKeys)
            {
                StartBinding(channel, queueName, exchangeName, routingKey);
            }
        }

        private static void StartBinding(IModel channel, string queueName, string exchangeName, string routingKey)
        {
            channel.QueueBind(queueName, exchangeName, routingKey);
        }

        public static IDictionary<string, object> GetSanitizedArguments(IDictionary<string, object> arguments)
        {
            return arguments?.ToDictionary(keyValue => keyValue.Key, SanitizeArgumentValue);
        }

        private static object SanitizeArgumentValue(KeyValuePair<string, object> keyValue)
        {
            switch (keyValue.Key)
            {
                case "x-max-length":
                    return Convert.ToInt32(keyValue.Value);

                case "x-message-ttl":
                    return Convert.ToInt32(keyValue.Value);

                case "x-expires":
                    return Convert.ToInt32(keyValue.Value);

                default:
                    return keyValue.Value;
            }
        }

       
    }
}