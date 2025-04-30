
using System.Collections.Concurrent;
using System.Net.Sockets;
using Infraestructura.Utils;

namespace Servidor;

public class ClienteManager
{
    private static readonly ConcurrentDictionary<int, Cliente> Clientes = new ConcurrentDictionary<int, Cliente>();
    

    public static void GestionarCliente(int clienteId, NetworkStream netwS)
    {
        Cliente clienteNuevo = new Cliente(clienteId, netwS);
        AñadirCliente(clienteNuevo);
        Consola.Success($"Handshake OK con vehículo #{clienteId}");
    }


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
        Consola.Info($"Clientes conectados: #{Clientes.Count}");
    }


    public static IEnumerable<Cliente> ClientesConectados()
    {
        return Clientes.Values;
    }
}