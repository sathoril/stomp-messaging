using System;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace WebSocketApplicationReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer("ws://localhost:15674");
            wssv.AuthenticationSchemes = AuthenticationSchemes.Basic;
            wssv.Realm = "WebSocket Test";
            wssv.UserCredentialsFinder = id =>
            {

                return new NetworkCredential("guest", "guest");
            };

            wssv.AddWebSocketService<Teste>("/teste");
            wssv.Start();


            if(wssv.IsListening)
            {
                Console.WriteLine("O Servidor de WebSocket está ouvindo na porta: 15674");
                foreach (var item in wssv.WebSocketServices.Paths)
                {
                    Console.WriteLine("- {0}", item);
                }
            }

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();

            wssv.Stop();
        }

        public class Teste : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                Console.WriteLine("Recebida msg");

                Send("Recebido");
            }
        }
    }
}
