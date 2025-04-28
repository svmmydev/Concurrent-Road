
using System.Net.Sockets;
using System.Threading.Tasks;
using CarreteraClass;
using NetworkStreamNS;
using PuenteClass;
using VehiculoClass;

namespace Servidor;

public class RecepcionVehiculoHandler
{
    private static object broadcastLock = new object();
    private static Puente puente = new Puente();


    public static async Task GestionarVehiculoAsync(NetworkStream netwS, Carretera carretera)
    {
        Vehiculo vehiculo = await netwS.LeerDatosVehiculoNSAsync();
        carretera.AñadirVehiculo(vehiculo);

        bool enPuente = false;

        while(!vehiculo.Acabado)
        {
            vehiculo = await netwS.LeerDatosVehiculoNSAsync();

            if (!enPuente && vehiculo.Pos == 39)
            {
                vehiculo.Parado = true;
                carretera.ActualizarVehiculo(vehiculo);
                EnviarEstadoCarretera(carretera);

                await puente.EntrarPuenteAsync(vehiculo);

                vehiculo.Parado = false;
                enPuente = true;
            }

            if (enPuente && vehiculo.Pos == 61)
            {
                puente.SalirPuente(vehiculo);
                enPuente = false;
            }

            carretera.ActualizarVehiculo(vehiculo);
            EnviarEstadoCarretera(carretera);
        }

        if (enPuente) puente.SalirPuente(vehiculo);
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