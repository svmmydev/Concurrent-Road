
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

                if (VehiculoHandler.vehiculo != null)
                {
                    var vehiculoEnServidor = carretera.VehiculosEnCarretera
                        .FirstOrDefault(v => v.Id == VehiculoHandler.vehiculo.Id);

                    if (vehiculoEnServidor != null)
                    {
                        VehiculoHandler.vehiculo.Parado = vehiculoEnServidor.Parado;
                        VehiculoHandler.vehiculo.Pos = vehiculoEnServidor.Pos;
                        VehiculoHandler.vehiculo.Acabado = vehiculoEnServidor.Acabado;
                    }
                }

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
                int posicion = vehiculo.Direccion == "Sur"
                    ? totalKm - vehiculo.Pos
                    : vehiculo.Pos;

                double porcentaje = (double)posicion / totalKm;
                int llenos  = (int)(porcentaje * tamañoBarra);
                int vacios  = tamañoBarra - llenos;

                string barra = vehiculo.Direccion == "Sur"
                    ? new string('▒', llenos) + new string('█', vacios)
                    : new string('█', llenos) + new string('▒', vacios);

                string estado = ObtenerEstadoVehiculo(posicion, vehiculo.Parado);

                Console.BackgroundColor = ObtenerColorEstado(posicion, vehiculo.Parado);

                Console.WriteLine($"Vehículo #{vehiculo.Id}: {barra} ({estado}) [{vehiculo.Direccion}]");

                Console.ResetColor();
            }
        }
    }


    private static string ObtenerEstadoVehiculo(int posicion, bool parado)
    {
        if (posicion == 0 || posicion == 100) return "Terminado";
        else if (!parado && posicion >= 39 && posicion <= 61) return $"Cruzando - {posicion} km";
        else if (parado && (posicion == 39 || posicion == 61)) return $"Esperando - {posicion} km";
        else return $"{posicion} km";
    }


    private static ConsoleColor ObtenerColorEstado(int posicion, bool parado)
    {
        if (posicion == 0 || posicion == 100) return ConsoleColor.Green;
        else if (!parado && posicion >= 39 && posicion <= 61) return ConsoleColor.Yellow;
        else if (parado && (posicion == 39 || posicion == 61)) return ConsoleColor.Red;
        else return ConsoleColor.Black;
    }
}