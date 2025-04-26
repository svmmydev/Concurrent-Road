
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
            Console.WriteLine($"ID Asignado: {clienteId}");

            await netwS.EscribirMensajeAsync(clienteId);
            Console.WriteLine("Handshake completado!");

            return clienteId;
        }
    }
}