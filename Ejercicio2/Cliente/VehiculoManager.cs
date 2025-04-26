
using System.Net.Sockets;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente;

public class VehiculoManager 
{
    public static void IniciarVehiculo(NetworkStream netwS, int id)
    {
        Vehiculo vehiculo = new Vehiculo();
        vehiculo.Id = id;

        netwS.EscribirDatosVehiculoNS(vehiculo);
    }
}