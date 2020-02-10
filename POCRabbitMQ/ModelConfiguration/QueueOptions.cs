using System.Collections.Generic;

namespace POCRabbitMQ
{
    public class QueueOptions
    {
        public string Name { get; set; }

        public bool Durable { get; set; } = true;

        public bool Exclusive { get; set; } = true;

        public bool AutoDelete { get; set; } = false;

        public HashSet<string> RoutingKeys { get; set; } = new HashSet<string>();

        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}