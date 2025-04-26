
using System.Net.Sockets;
using CarreteraClass;
using NetworkStreamNS;
using VehiculoClass;

namespace Servidor;

public class RecepcionVehiculoHandler
{
    public static void GestionarVehiculo(NetworkStream netwS, Carretera carretera)
    {
        Vehiculo vehiculo = NetworkStreamClass.LeerDatosVehiculoNS(netwS);
        carretera.AñadirVehiculo(vehiculo);
        carretera.MostrarVehiculos();
        Console.WriteLine($"Vehículos en carretera: {carretera.NumVehiculosEnCarrera}");
    }
}