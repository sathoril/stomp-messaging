using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace WebSocketApplicationWorker
{
    public class MessageHandler : DefaultBasicConsumer
    {
        private IModel rabbitmqChannel;

        public MessageHandler(RabbitMQ.Client.IModel rabbitmqChannel)
        {
            this.rabbitmqChannel = rabbitmqChannel;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            string message = Encoding.UTF8.GetString(body);

            Debug.WriteLine($"Reading messages from queue: {consumerTag}");
            Debug.WriteLine($"----------------------------------------------------");
            Debug.WriteLine($"Message read --> {message}");

            this.rabbitmqChannel.BasicAck(deliveryTag, false);
        }
    }
}
