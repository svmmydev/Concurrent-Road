
using System.Net.Sockets;

namespace Receptor
{
    class Receptor
    {
        public int? Id {get;set;}
        public NetworkStream NS {get;set;}
        public byte[] Buffer {get;set;}

        public Receptor(TcpClient cliente)
        {
            Id = null;
            NS = cliente.GetStream();
            Buffer = new byte[1024];
        }
    }
}