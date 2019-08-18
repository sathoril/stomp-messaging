using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocketApplicationSender.Classes
{
    /// <summary>
    /// Classe que contém os FRAMES Commands do Protocolo STOMP utilizado.
    /// FRAMES são colocados no header das requisições, de forma que ficam linkados com os dados da requisição. Os frams neste caso são referências aos comandos enviados com a requisição, por exemplo:
    /// Uma requisição está com o frame CONNECT, o que significa que a requisição tem como finalidade se conectar com o receptor da requisição.
    /// </summary>
    public class StompFrames
    {
        // Client Frames
        public const string CONNECT = "CONNECT";
        public const string DISCONNECT = "DISCONNECT";
        public const string SUBSCRIBE = "SUBSCRIBE";
        public const string UNSUBSCRIBE = "UNSUBSCRIBE";
        public const string SEND = "SEND";

        // Server Frames
        public const string CONNECTED = "CONNECTED";
        public const string MESSAGE = "MESSAGE";
        public const string ERROR = "ERROR";
    }
}
