
using System.Net.Sockets;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente;

public class VehiculoManager 
{
    private static Vehiculo? vehiculo;

    public static Vehiculo IniciarVehiculo(NetworkStream netwS, int clienteId)
    {
        vehiculo = new Vehiculo();
        vehiculo.Id = clienteId;

        netwS.EscribirDatosVehiculoNS(vehiculo);

        return vehiculo;
    }
}