
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Servidor
{
    public class ClienteManager
    {
        private static readonly ConcurrentDictionary<int, Cliente> Clientes = new ConcurrentDictionary<int, Cliente>();

        public static void AñadirCliente(Cliente cliente)
        {
            Clientes.TryAdd(cliente.ClienteId, cliente);
        }

        public static void EliminarCliente(int clienteId)
        {
            Clientes.TryRemove(clienteId, out _);
        }

        public static void MostrarClientesConectados()
        {
            Console.WriteLine($"Vehículos conectados: {Clientes.Count}");
        }
    }
}