using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using POCRabbitMQ;
namespace RabbitMQWEBApplication
{

    public class ConnectAPI
    {
        private readonly IConnectionsManager _connectionsManager;
        IConfiguration _iconfiguration;

       // private IOptions<RabbitMQOptions> options;
        public ConnectAPI(IConnectionsManager connectionsManager)
        {
            _connectionsManager = connectionsManager;


        }
        public ConnectAPI()
        {
            //_connectionsManager = new ConnectionsManager() ;
            //_iconfiguration = iconfiguration;
        }
        
        public async Task<IRabbitChannel> GetProductiondetails()
        {
            //var username = _iconfiguration.GetSection("RabbitMQ:Credentials:Username").Value;

            var channe = await _connectionsManager.GetOrEstablishChannelAsync();
            return channe;
        }

    }
}
