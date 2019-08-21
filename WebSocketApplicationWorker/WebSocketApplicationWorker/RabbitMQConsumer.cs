using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WebSocketApplicationWorker
{
    public class RabbitMQConsumer : IHostedService
    {
        private readonly String QueueName = "teste";

        private IConnection rabbitmqConnection;
        private IModel rabbitmqChannel;

        private Task executionTask;

        public RabbitMQConsumer()
        {
            this.StartRabbitmqConsumer();
        }

        public void StartRabbitmqConsumer()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            // Creates the connection with RabbitMQ Server
            this.rabbitmqConnection = factory.CreateConnection();

            // Create channel  
            this.rabbitmqChannel = this.rabbitmqConnection.CreateModel();

            // Implements the event when the connections is shutdown
            this.rabbitmqConnection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        /// <summary>
        /// Starts the service and configure the Exchange and Channel to be used on this RabbitMQ consumer
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.executionTask = ExecuteAsync(cancellationToken);

            if (this.executionTask.IsCompleted)
                return this.executionTask;

            return Task.FromException(new Exception("An error occurred while processing the queue."));
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Checks if the queue exists by the name passed as parameter, if not, creates it
            this.rabbitmqChannel.QueueDeclare(queue: QueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Initiate the class that will handle the messages being consumed by the EventingBasicConsumer above
            var messageHandler = new MessageHandler(this.rabbitmqChannel);

            // Consume messages from the queue
            this.rabbitmqChannel.BasicConsume(this.QueueName, false, messageHandler);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            if (!this.rabbitmqChannel.IsClosed)
                this.rabbitmqChannel.Close();

            if (!this.rabbitmqConnection.IsOpen)
                this.rabbitmqConnection.Close();

            throw new NotImplementedException();
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"The connection to the queue \"{QueueName}\" is being shutdown!");
        }
    }
}
