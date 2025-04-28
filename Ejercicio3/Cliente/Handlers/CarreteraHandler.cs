
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
                int displayPos = vehiculo.Direccion == "Sur"
                    ? totalKm - vehiculo.Pos
                    : vehiculo.Pos;

                double porcentaje = (double)displayPos / totalKm;
                int llenos  = (int)(porcentaje * tamañoBarra);
                int vacios  = tamañoBarra - llenos;

                string barra = vehiculo.Direccion == "Sur"
                    ? new string('▒', llenos) + new string('█', vacios)
                    : new string('█', llenos) + new string('▒', vacios);

                string estado;

                if (displayPos == 0 || displayPos == 100)
                {
                    estado = "Terminado";
                }
                else if (displayPos >= 40 && displayPos <= 60)
                {
                    estado = $"Cruzando - {displayPos} km";
                }
                else if (displayPos == 39 || displayPos == 61)
                {
                    estado = $"Esperando - {displayPos} km";
                }
                else
                {
                    estado = $"{displayPos} km";
                }

                Console.WriteLine($"Vehículo #{vehiculo.Id}: {barra} ({estado}) [{vehiculo.Direccion}]");
            }
        }
    }
}