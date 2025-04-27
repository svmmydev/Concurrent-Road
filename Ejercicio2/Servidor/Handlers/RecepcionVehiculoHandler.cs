
using System.Net.Sockets;
using CarreteraClass;
using NetworkStreamNS;
using VehiculoClass;

namespace Servidor;

public class RecepcionVehiculoHandler
{
    public static void GestionarVehiculo(NetworkStream netwS, Carretera carretera)
    {
        Vehiculo vehiculo = netwS.LeerDatosVehiculoNS();
        carretera.AñadirVehiculo(vehiculo);
        
        // TODO MOSTRAR SOLO EN CLIENTE CUANDO TENGAMOS TODO LO QUE FALTA (AQUI ES PARA PRUEBA SOLO, REPESCAR EL MÉTODO ANTIGUO)

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