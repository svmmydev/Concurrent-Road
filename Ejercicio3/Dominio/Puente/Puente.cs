using Infraestructura.Utils;
using VehiculoClass;

namespace PuenteClass;

public static class Puente
{
    private static readonly SemaphoreSlim semaforoPuente = new SemaphoreSlim(1, 1);
    private static readonly PriorityQueue<Vehiculo, int> colaEspera = new();
    private static readonly object lockCola = new();


    /// <summary>
    /// Solicita acceso al puente. El vehículo se encola según su prioridad y espera su turno.
    /// Solo se permite un vehículo al mismo tiempo en el puente.
    /// </summary>
    /// <param name="vehiculo">Vehículo que desea cruzar el puente.</param>
    /// <returns>Una tarea asincrónica que finaliza cuando el vehículo entra al puente.</returns>
    public static async Task EntrarPuenteAsync(Vehiculo vehiculo)
    {
        int prioridad = vehiculo.Direccion == "Sur" ? 0 : 1;

        lock (lockCola)
        {
            colaEspera.Enqueue(vehiculo, prioridad);
        }

        Consola.Warn($"Vehículo {vehiculo.Id} ({vehiculo.Direccion}) en cola para cruzar..");

        while (true)
        {
            Vehiculo siguiente;

            lock (lockCola)
            {
                if (colaEspera.TryPeek(out siguiente, out _)
                    && siguiente.Id == vehiculo.Id
                    && semaforoPuente.CurrentCount > 0)
                {
                    colaEspera.Dequeue();
                    break;
                }
            }

            await Task.Delay(50);
        }

        await semaforoPuente.WaitAsync();
        Consola.Warn($"Vehículo {vehiculo.Id} ha entrado al puente");
    }


    /// <summary>
    /// Libera el semáforo del puente una vez que el vehículo ha terminado de cruzar.
    /// </summary>
    /// <param name="vehiculoId">Identificador del vehículo que sale del puente.</param>
    public static void SalirPuente(int vehiculoId)
    {
        Consola.Success($"Vehículo {vehiculoId} ha salido del puente");
        semaforoPuente.Release();
    }
}
