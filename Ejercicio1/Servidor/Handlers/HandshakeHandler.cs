
using System.Net.Sockets;

namespace Servidor.Handlers
{
    public class HandshakeHandler
    {
        private readonly TcpListener servidor;
        private int clienteId;

        public HandshakeHandler(TcpListener servidor)
        {
            this.servidor = servidor;
        }

        // TO:DO Encapsulación del Handshake
        
    }
}