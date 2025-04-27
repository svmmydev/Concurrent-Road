
using System.Net.Sockets;
using CarreteraClass;
using NetworkStreamNS;
using VehiculoClass;

namespace Servidor;

public class RecepcionVehiculoHandler
{
    private static object broadcastLock = new object();


    public static void GestionarVehiculo(NetworkStream netwS, Carretera carretera)
    {
        Vehiculo vehiculo = netwS.LeerDatosVehiculoNS();
        carretera.AñadirVehiculo(vehiculo);

        while(!vehiculo.Acabado)
        {
            vehiculo = netwS.LeerDatosVehiculoNS();
            carretera.ActualizarVehiculo(vehiculo);
            
            EnviarEstadoCarretera(carretera);
        }


    }


    public static void EnviarEstadoCarretera(Carretera carretera)
    {
        lock(broadcastLock)
        {
            foreach (Cliente cliente in ClienteManager.ClientesConectados())
            {
                try
                {
                    cliente.ClienteNetwS.EscribirDatosCarreteraNS(carretera);
                }
                catch
                {
                    ClienteManager.EliminarCliente(cliente.ClienteId);
                    Console.WriteLine($"\n# El cliente con id {cliente.ClienteId} se ha desconectado del servidor #");
                }
            }
        }
    }
}