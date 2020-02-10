using System;
using System.Collections.Generic;
using System.Text;

namespace POCRabbitMQ.ModelConfiguration
{
   public class MessageTypeRecord
    {
        public string QueueNameOrId { get; set; }

        public string RoutingKey { get; set; }

        public bool HandlerAcks { get; set; }

        public Predicate<string> Predicate { get; set; }

        //public IMediatedHandler Handler { get; set; }
    }
}
