
using System.Net.Sockets;

namespace Servidor
{
    public class Cliente
    {
        public int ClienteId {get;}
        public NetworkStream ClienteNetwS {get;}

        public Cliente(int clienteId, NetworkStream clienteNetwS)
        {
            ClienteId = clienteId;
            ClienteNetwS = clienteNetwS;
        }
    }
}