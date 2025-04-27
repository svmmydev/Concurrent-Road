
using System.Net.Sockets;
using CarreteraClass;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente.Handlers;

public class CarreteraHandler
{
    private static Carretera carretera = new Carretera();

    public static void ActualizarCarretera(NetworkStream netwS)
    {
        try
        {
            while (true)
            {
                carretera = netwS.LeerDatosCarreteraNS();

                Console.Clear();
                carretera.MostrarCarretera();
            }
        }
        catch (IOException)
        {
            Console.WriteLine("El servidor cerró el stream o hubo un error de red: Saliendo..");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error recibiendo carretera: {ex.Message}");
        }
    }
}