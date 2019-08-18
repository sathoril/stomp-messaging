using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketApplicationSender.Classes;
using WebSocketSharp;

namespace WebSocketApplicationSender
{
    public class Program
    {

        static void Main(string[] args)
        {
            try
            {
                string nomeDaFila = "teste";
                string nomeDoVirtualHost = "/";
                bool exit = false;

                var stompProtocol = new StompProtocol(nomeDaFila);

                stompProtocol.OpenConnection("guest", "guest");

                for (int i = 0; i < 100; i++)
                {
                    if (stompProtocol.IsConnectionOpen())
                    {
                        Thread.Sleep(5000);
                        stompProtocol.SendMessage($"Mensagem {i.ToString()}...");
                    }
                }

                stompProtocol.CloseConnection();
            }
            catch(Exception ex)
            {
                throw ex;
            }


            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //factory.Port = 15674;

            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare(queue: "teste",
            //                             durable: false,
            //                             exclusive: false,
            //                             autoDelete: false,
            //                             arguments: null);

            //        string message = "Hello World!";
            //        var body = Encoding.UTF8.GetBytes(message);

            //        channel.BasicPublish(exchange: "",
            //                             routingKey: "hello",
            //                             basicProperties: null,
            //                             body: body);
            //        Console.WriteLine(" [x] Sent {0}", message);
            //    }
            //}

            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();
        }
    }
}