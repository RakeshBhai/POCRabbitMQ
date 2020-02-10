using RabbitMQ.Client;
using System;

namespace POCRabbitMQ
{
    public class SettingsOptions
    {
        public bool AutomaticRecoveryEnabled { get; set; } = true;

        public bool TopologyRecoveryEnabled { get; set; } = true;

        public int RequestedConnectionTimeout { get; set; } = ConnectionFactory.DefaultConnectionTimeout;

        public ushort RequestedChannelMax { get; set; } = ConnectionFactory.DefaultChannelMax;

        public uint RequestedFrameMax { get; set; } = ConnectionFactory.DefaultFrameMax;

        public ushort RequestedHeartbeat { get; set; } = ConnectionFactory.DefaultHeartbeat;

        public int MessageHandlerStartupDelay { get; set; } = 500;
    }
}
