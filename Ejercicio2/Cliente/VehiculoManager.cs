
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


    public static void MoverVehiculo(NetworkStream netwS)
    {
        for (int i = 0; i <= 100; i++)
        {
            Thread.Sleep(vehiculo.Velocidad);
            vehiculo.Pos =+ i;
            NetworkStreamClass.EscribirDatosVehiculoNS(netwS, vehiculo);
        }

        vehiculo.Acabado = true;
    }
}