
using System.Net.Sockets;
using System.Threading.Tasks;
using CarreteraClass;
using NetworkStreamNS;
using VehiculoClass;

namespace Servidor;

public class RecepcionVehiculoHandler
{
    public static void GestionarVehiculo(NetworkStream netwS, Carretera carretera, int linea)
    {
        Vehiculo vehiculo = netwS.LeerDatosVehiculoNS();
        carretera.AñadirVehiculo(vehiculo);
        carretera.MostrarCarretera();

        while(!vehiculo.Acabado)
        {
            vehiculo = netwS.LeerDatosVehiculoNS();
            carretera.ActualizarVehiculo(vehiculo);
            carretera.MostrarCarretera();
            Thread.Sleep(200);
        }
    }
}