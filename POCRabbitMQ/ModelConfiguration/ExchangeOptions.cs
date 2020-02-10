using System.Collections.Generic;
using System.Collections;

namespace POCRabbitMQ
{
    public class ExchangeOptions
    {
        public string Name { get; set; } = "main_exchange";

        public string Type { get; set; } = "direct";

        public bool Durable { get; set; } = true;

        public bool Exclusive { get; set; }

        public bool AutoDelete { get; set; }

        public string DeadLetterExchange { get; set; } = "default.dlx.exchange";

        public bool RequeueFailedMessages { get; set; } = true;

        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();

        public IDictionary<string, QueueOptions> Queues { get; set; } = new Dictionary<string, QueueOptions>();

        public bool IsDefault { get; set; }
    }
}