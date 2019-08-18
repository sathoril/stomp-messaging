using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebSocketSharp;

namespace WebSocketApplicationSender.Classes
{
    public class StompProtocol
    {
        private static string WebSocketUrl = "ws://localhost:15674/ws";
        private static StompMessageSerializer serializer = new StompMessageSerializer();

        private string queueName;
        private string virtualHost;

        WebSocket webSocket = null;

        /// <summary>
        /// Construtor sem o parâmetro com o nome do virtual host, que é setado como o virtual host default igual a "/"
        /// </summary>
        /// <param name="queueName"></param>
        public StompProtocol(string queueName)
            : this(queueName, "/")
        {

        }

        public StompProtocol(string queueName, string virtualHost)
        {
            this.webSocket = new WebSocket(WebSocketUrl);
            this.queueName = queueName;
            this.virtualHost = virtualHost;
        }

        public void OpenConnection(string username, string password)
        {
            Console.WriteLine("------- Conectando WebStompProtocol...");
            Thread.Sleep(500);

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("------- Setando credenciais de acesso...");
            Console.WriteLine("--------------------------------------");
            Thread.Sleep(500);
            webSocket.SetCredentials(username, password, false);
            


            Console.WriteLine("--------------------------------------");
            Console.WriteLine("------- Configurando eventos...");
            Console.WriteLine("--------------------------------------");
            Thread.Sleep(500);
            webSocket.OnMessage += ws_OnMessage;
            webSocket.OnClose += ws_OnClose;
            webSocket.OnOpen += ws_OnOpen;
            webSocket.OnError += ws_OnError;

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("------- Tentando conexão...");
            Console.WriteLine("--------------------------------------");
            Thread.Sleep(500);
            webSocket.Connect();

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("-------Conexão aberta!");
            Console.WriteLine("--------------------------------------");

        }

        public Boolean IsConnectionOpen()
        {
            return this.webSocket.ReadyState == WebSocketState.Open;
        }

        public void CloseConnection()
        {
            this.webSocket.Close();
        }

        public void SendMessage(string message)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"------- Enviando mensagem para a fila {this.queueName}...");
            Console.WriteLine("--------------------------------------");

            var content = new { Subject = "Stomp client", Message = message };
            var broad = new StompMessage(StompFrames.SEND, JsonConvert.SerializeObject(content));

            broad["content-type"] = "application/json";

            // Nome da Fila
            broad["destination"] = $"/queue/{queueName}";

            webSocket.Send(serializer.Serialize(broad));

            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"------- Mensagem enviada para a fila {this.queueName}...");
            Console.WriteLine("--------------------------------------");
        }

        #region [ Stomp Events ]

        private void ws_OnOpen(object sender, EventArgs e)
        {
            ConnectStomp();
        }

        private void ConnectStomp()
        {
            var connect = new StompMessage(StompFrames.CONNECT);
            connect["accept-version"] = "1.2";
            connect["host"] = "/";

            // First number Zero mean client not able to send Heartbeat, 
            // Second number mean Server will sending heartbeat to client instead
            connect["heart-beat"] = "0,10000";
            webSocket.Send(serializer.Serialize(connect));
        }

        private void SubscribeStomp()
        {
            var subscription = new StompMessage(StompFrames.SUBSCRIBE);

            subscription["id"] = "subscription";
            subscription["destination"] = $"/queue/{queueName}";
            webSocket.Send(serializer.Serialize(subscription));
        }

        private void ws_OnMessage(object sender, MessageEventArgs e)
        {
            StompMessage msg = serializer.Deserialize(e.Data);

            if (msg.Command == StompFrames.CONNECTED)
            {
                SubscribeStomp();
            }
            else if (msg.Command == StompFrames.MESSAGE)
            {
                Console.WriteLine(e.Data);
            }
        }

        private void ws_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("------- Fechando conexão com WebStompProtocol...");
            DisconnectStomp();
            Console.WriteLine("------- Conexão com WebStompProtocol fechada!");
        }

        private void DisconnectStomp()
        {
            webSocket.Close();

            //var connect = new StompMessage(StompFrames.DISCONNECT);
            //connect["accept-version"] = "1.2";
            //connect["host"] = $"{virtualHost}";

            //// First number Zero mean client not able to send Heartbeat, 
            //// Second number mean Server will sending heartbeat to client instead
            //connect["heart-beat"] = "0,10000";
            //webSocket.Send(serializer.Serialize(connect));
        }

        private void ws_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"------- Ocorreu um erro na execução do WebStompProtocol, detalhes: {e.Message}..."); 
        }

        #endregion
    }
}
