
using System.Net.Sockets;

namespace Servidor;

public class Cliente
{
    public int ClienteId {get;}
    public NetworkStream ClienteNetwS {get;}
    

    /// <summary>
    /// Inicializa una nueva instancia de la clase Cliente con su identificador y flujo de red asociado.
    /// </summary>
    /// <param name="clienteId">Identificador único del cliente.</param>
    /// <param name="clienteNetwS">Flujo de red asociado a la conexión del cliente.</param>
    public Cliente(int clienteId, NetworkStream clienteNetwS)
    {
        ClienteId = clienteId;
        ClienteNetwS = clienteNetwS;
    }
}