
using System.Net.Sockets;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente.Handlers;

public class VehiculoHandler 
{
    public static Vehiculo? vehiculo;


    /// <summary>
    /// Inicializa un nuevo vehículo con el ID del cliente y lo envía al servidor a través del <see cref="NetworkStream"/>.
    /// </summary>
    /// <param name="netwS">Stream de red utilizado para comunicar con el servidor.</param>
    /// <param name="clienteId">Identificador único del cliente.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve el vehículo creado.</returns>
    public static async Task<Vehiculo> IniciarVehiculoAsync(NetworkStream netwS, int clienteId)
    {
        vehiculo = new Vehiculo();
        vehiculo.Id = clienteId;

        await netwS.EscribirDatosVehiculoNSAsync(vehiculo);

        return vehiculo;
    }


    /// <summary>
    /// Controla el movimiento del vehículo en la carretera. 
    /// Si el vehículo no está parado, avanza su posición y actualiza su estado en el servidor.
    /// Finaliza cuando el vehículo ha alcanzado el final o el inicio de la carretera.
    /// </summary>
    /// <param name="netwS">Stream de red utilizado para enviar actualizaciones del vehículo.</param>
    /// <returns>Una tarea que representa la operación asincrónica de movimiento.</returns>
    public static async Task MoverVehiculoAsync(NetworkStream netwS)
    {
        while (vehiculo?.Pos >= 0 && vehiculo.Pos <= 100 && !vehiculo.Acabado)
        {
            if (!vehiculo.Parado)
            {
                vehiculo.Pos += 1;

                if (vehiculo.Pos == 100 || vehiculo.Pos == 0) vehiculo.Acabado = true;
                
                await netwS.EscribirDatosVehiculoNSAsync(vehiculo);
            }

            await Task.Delay(vehiculo.Velocidad);
        }
        
        await netwS.EscribirDatosVehiculoNSAsync(vehiculo!);
    }
}