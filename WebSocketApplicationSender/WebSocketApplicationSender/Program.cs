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
        }
    }
}