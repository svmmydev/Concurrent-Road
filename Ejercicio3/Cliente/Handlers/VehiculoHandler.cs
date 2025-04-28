
using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente.Handlers;

public class VehiculoHandler 
{
    public static Vehiculo? vehiculo;

    public static async Task<Vehiculo> IniciarVehiculoAsync(NetworkStream netwS, int clienteId)
    {
        vehiculo = new Vehiculo();
        vehiculo.Id = clienteId;

        await netwS.EscribirDatosVehiculoNSAsync(vehiculo);

        return vehiculo;
    }

    public static async Task MoverVehiculoAsync(NetworkStream netwS)
    {
        while (vehiculo.Pos >= 0 && vehiculo.Pos <= 100 && !vehiculo.Acabado)
        {
            if (!vehiculo.Parado)
            {
                vehiculo.Pos += 1;

                if (vehiculo.Pos == 100 || vehiculo.Pos == 0) vehiculo.Acabado = true;
                
                await netwS.EscribirDatosVehiculoNSAsync(vehiculo);
            }

            await Task.Delay(vehiculo.Velocidad);
        }
        
        vehiculo.Acabado = true;
        await netwS.EscribirDatosVehiculoNSAsync(vehiculo);
    }
}