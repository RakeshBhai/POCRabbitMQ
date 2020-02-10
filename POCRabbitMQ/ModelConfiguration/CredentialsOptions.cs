using RabbitMQ.Client;
using System;

namespace POCRabbitMQ
{
    public class CredentialsOptions
    {
        //public string Username { get; set; } = ConnectionFactory.DefaultUser;
        //public string Password { get; set; } = ConnectionFactory.DefaultPass;

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
