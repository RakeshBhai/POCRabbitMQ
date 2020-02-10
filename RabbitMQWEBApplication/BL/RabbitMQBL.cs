using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using POCRabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQWEBApplication.BL
{
    public class RabbitMQBL: IRabbitMQBL
    {
        #region variable
        private readonly IConfiguration _config;
        private readonly IConnectionsManager _connectionsManager;
        private readonly IOptions<RabbitMQOptions> _rabbitMQOptions;
        #endregion
        public RabbitMQBL(IConfiguration config, IConnectionsManager connectionsManager, IOptions<RabbitMQOptions> RabbitMQOptions)
        {
            _config = config;
            _connectionsManager = connectionsManager;
            _rabbitMQOptions = RabbitMQOptions;
        }
        public async Task GetAsny()
        {
            //await _connectionsManager.SendTextAsync("Hello World", "Exchanges", "routingKey");
            await _connectionsManager.SendAsync("obj", "Exchanges", "routingKey");

        }

    }
}
