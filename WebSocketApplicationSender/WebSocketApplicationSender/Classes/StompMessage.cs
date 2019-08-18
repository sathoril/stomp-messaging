using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocketApplicationSender.Classes
{
    /// <summary>
    /// A mensagem do protocolo STOMP deve seguir o seguinte formato:
    /// COMMAND         -- FRAME
    /// header1:value1  -- HEADER (No formato Key:Value)
    /// header2:value2  -- HEADER (No formato Key:Value)
    /// Body^@          -- BODY seguido de um null byte representado na tabela ASCII por control-@
    /// </summary>
    public class StompMessage
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        public String Command { get; private set; }
        public String Body { get; private set; }
        
        /// <summary>
        /// Seta ou Obtém algum atributo que está contido no header
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public string this[string header]
        {
            get { return this.Headers.ContainsKey(header) ? this.Headers[header] : string.Empty; }
            set { this.Headers[header] = value; }
        }
        
        public StompMessage(string commandFrame)
            : this(commandFrame, String.Empty, new Dictionary<string, string>()) { }

        public StompMessage(string commandFrame, string mensagem)
            : this(commandFrame, String.Empty, new Dictionary<string, string>()) { }

        public StompMessage(string commandFrame, string bodyMessage, Dictionary<string, string> headers)
        {
            this.Command = commandFrame;
            this.Body = bodyMessage;
            this._headers = headers;

            this["content-length"] = bodyMessage.Length.ToString();
        }

        public Dictionary<string, string> Headers
        {
            get { return this._headers; }
        }
    }
}
 