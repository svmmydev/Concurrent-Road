
using System.Collections.Concurrent;
using System.Net.Sockets;
using Infraestructura.Utils;

namespace Servidor;

public class ClienteManager
{
    private static readonly ConcurrentDictionary<int, Cliente> Clientes = new ConcurrentDictionary<int, Cliente>();
    

    /// <summary>
    /// Registra un nuevo cliente creando su instancia y añadiéndolo a la colección de clientes conectados.
    /// También informa por consola que el handshake se completó correctamente.
    /// </summary>
    /// <param name="clienteId">Identificador único del cliente.</param>
    /// <param name="netwS">Flujo de red asociado a ese cliente.</param>
    public static void GestionarCliente(int clienteId, NetworkStream netwS)
    {
        Cliente clienteNuevo = new Cliente(clienteId, netwS);
        AñadirCliente(clienteNuevo);
    }

    
    /// <summary>
    /// Añade un cliente a la lista de clientes conectados de forma segura.
    /// </summary>
    /// <param name="cliente">Instancia del cliente a añadir.</param>
    public static void AñadirCliente(Cliente cliente)
    {
        Clientes.TryAdd(cliente.ClienteId, cliente);
    }


    /// <summary>
    /// Elimina un cliente de la lista de clientes conectados según su identificador.
    /// </summary>
    /// <param name="clienteId">ID del cliente a eliminar.</param>
    public static void EliminarCliente(int clienteId)
    {
        Clientes.TryRemove(clienteId, out _);
    }

    
    /// <summary>
    /// Muestra por consola cuántos clientes están actualmente conectados al servidor.
    /// </summary>
    public static void MostrarClientesConectados()
    {
        Consola.Info($"Clientes conectados: #{Clientes.Count}");
    }


    /// <summary>
    /// Devuelve una colección de todos los clientes actualmente conectados.
    /// </summary>
    /// <returns>Colección enumerable de clientes activos.</returns>
    public static IEnumerable<Cliente> ClientesConectados()
    {
        return Clientes.Values;
    }
}