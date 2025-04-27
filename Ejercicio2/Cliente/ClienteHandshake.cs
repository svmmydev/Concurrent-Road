
using System.Net.Sockets;
using NetworkStreamNS;

namespace Cliente
{
    public static class ClienteHandshake
    {
        public static async Task<string> InicioHandshakeCliente(NetworkStream netwS)
        {
            await netwS.EscribirMensajeAsync("INICIO");

            string clienteId = await netwS.LeerMensajeAsync();

            await netwS.EscribirMensajeAsync(clienteId);

            return clienteId;
        }
    }
}