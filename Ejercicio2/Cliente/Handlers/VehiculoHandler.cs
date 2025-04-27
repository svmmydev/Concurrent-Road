
using System.Net.Sockets;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente.Handlers;

public class VehiculoHandler 
{
    private static Vehiculo? vehiculo;

    public static Vehiculo IniciarVehiculo(NetworkStream netwS, int clienteId)
    {
        vehiculo = new Vehiculo();
        vehiculo.Id = clienteId;

        netwS.EscribirDatosVehiculoNS(vehiculo);

        return vehiculo;
    }

    public static void MoverVehiculo(NetworkStream netwS)
    {
        for (int i = 0; i <= 100; i++)
        {
            Thread.Sleep(vehiculo.Velocidad);
            vehiculo.Pos += 1;
            NetworkStreamClass.EscribirDatosVehiculoNS(netwS, vehiculo);
        }
        
        vehiculo.Acabado = true;
        NetworkStreamClass.EscribirDatosVehiculoNS(netwS, vehiculo);
    }
}