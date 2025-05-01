
using System.Net.Sockets;
using CarreteraClass;
using NetworkStreamNS;
using VehiculoClass;
using Servidor;

namespace Servidor;

public class RecepcionVehiculoHandler
{
    private static object broadcastLock = new object();


    public static void GestionarVehiculo(Cliente cliente, Carretera carretera)
    {
        try
        {
            Vehiculo vehiculo = cliente.ClienteNetwS.LeerDatosVehiculoNS();
            carretera.AñadirVehiculo(vehiculo);
            carretera.MostrarVehiculos();

            while(!vehiculo.Acabado)
            {
                vehiculo = cliente.ClienteNetwS.LeerDatosVehiculoNS();
                carretera.ActualizarVehiculo(vehiculo);
                
                EnviarEstadoCarretera(carretera);
            }
        }
        catch (IOException)
        {
            Console.WriteLine($"# ERROR: Fallo de lectura en vehículo #{cliente.ClienteId}");
        }
        finally
        {
            ClienteManager.EliminarCliente(cliente.ClienteId);
            Console.WriteLine($"\n# El cliente con id {cliente.ClienteId} se ha desconectado del servidor #");

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