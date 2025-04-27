
using System.Net.Sockets;
using CarreteraClass;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente.Handlers;

public class CarreteraHandler
{
    private static Carretera carretera = new Carretera();
    const int totalKm = 100;
    const int tamañoBarra = 20;
    static object consoleLock = new object();

    public static void ActualizarCarretera(NetworkStream netwS)
    {
        try
        {
            while (true)
            {
                carretera = netwS.LeerDatosCarreteraNS();

                Console.Clear();
                MostrarCarretera();
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

    //Muestra por pantalla la carretera. 
    private static void MostrarCarretera()
    {
        lock(consoleLock)
        {
            Console.Clear();

            foreach (Vehiculo vehiculo in carretera.VehiculosEnCarretera.OrderBy(x => x.Id))
            {
                double porcentaje = (double)vehiculo.Pos / totalKm;
                int llenos = (int)(porcentaje * tamañoBarra);
                int vacios = tamañoBarra - llenos;

                string carretera = new string('█', llenos) + new string('▒', vacios);

                string estado = vehiculo.Pos >= totalKm ? "Terminado"
                                                        : $"{vehiculo.Pos} km";

                Console.WriteLine($"Vehículo #{vehiculo.Id}: {carretera} ({estado}) [{vehiculo.Direccion}]");
            }
        }
    }
}