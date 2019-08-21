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

                for(int i=0; i < 1000; i++)
                { 
                    if (stompProtocol.IsConnectionOpen())
                    {
                        Thread.Sleep(500);

                        double randomNumber = new Random().Next();

                        if(randomNumber % 2 == 0)
                            stompProtocol.SendMessage($"Mensagem {randomNumber.ToString()}...");
                    }
                    else
                    {
                        stompProtocol.OpenConnection("guest", "guest");
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