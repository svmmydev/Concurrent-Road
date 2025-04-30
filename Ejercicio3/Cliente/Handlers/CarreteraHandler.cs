
using System.Net.Sockets;
using CarreteraClass;
using Infraestructura.Utils;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente.Handlers;

public class CarreteraHandler
{
    private static Carretera carretera = new Carretera();
    const int totalKm = 100;
    const int tamañoBarra = 20;
    static object consoleLock = new object();


    /// <summary>
    /// Recibe continuamente el estado actualizado de la carretera desde el servidor a través de un NetworkStream.
    /// Actualiza el estado del vehículo local si está presente y muestra la carretera en consola.
    /// </summary>
    /// <param name="netwS">Stream de red conectado al servidor.</param>
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

                MostrarCarretera();
            }
        }
        catch (IOException)
        {
            Consola.Error("El servidor cerró el stream o hubo un error de red: Saliendo..");
        }
        catch (Exception ex)
        {
            Consola.Error($"Error recibiendo carretera: {ex.Message}");
        }
    }


    /// <summary>
    /// Muestra en consola una representación visual del estado actual de la carretera.
    /// Indica si cada vehículo está conectado o desconectado, usando colores y símbolos.
    /// Vehículos conectados muestran una barra de progreso con su estado; 
    /// los desconectados se muestran con un mensaje especial en gris.
    /// </summary>
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

                if (vehiculo.Desconectado)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[✗] Desconectado: [ FALLO INESPERADO ] - Vehículo #{vehiculo.Id}");
                }
                else
                {
                    Console.BackgroundColor = ObtenerColorEstado(posicion, vehiculo.Parado);
                    Console.WriteLine($"[✓] Conectado: {barra} ({estado}) [{vehiculo.Direccion}] - Vehículo #{vehiculo.Id}");
                }

                Console.ResetColor();
            }
        }
    }


    /// <summary>
    /// Devuelve un string que representa el estado actual de un vehículo según su posición y si está parado.
    /// </summary>
    /// <param name="posicion">Posición del vehículo en kilómetros.</param>
    /// <param name="parado">Indica si el vehículo está detenido.</param>
    /// <returns>Una descripción del estado del vehículo.</returns>
    private static string ObtenerEstadoVehiculo(int posicion, bool parado)
    {
        if (posicion == 0 || posicion == 100) return "Terminado";
        else if (!parado && posicion >= 39 && posicion <= 61) return $"Cruzando - {posicion} km";
        else if (parado && (posicion == 39 || posicion == 61)) return $"Esperando - {posicion} km";
        else return $"{posicion} km";
    }


    /// <summary>
    /// Determina el color de fondo que se debe usar en consola para representar visualmente el estado del vehículo.
    /// </summary>
    /// <param name="posicion">Posición del vehículo en kilómetros.</param>
    /// <param name="parado">Indica si el vehículo está detenido.</param>
    /// <returns>Un valor de ConsoleColor que representa el estado del vehículo.</returns>
    private static ConsoleColor ObtenerColorEstado(int posicion, bool parado)
    {
        if (posicion == 0 || posicion == 100) return ConsoleColor.Green;
        else if (!parado && posicion >= 39 && posicion <= 61) return ConsoleColor.Yellow;
        else if (parado && (posicion == 39 || posicion == 61)) return ConsoleColor.Red;
        else return ConsoleColor.Black;
    }
}