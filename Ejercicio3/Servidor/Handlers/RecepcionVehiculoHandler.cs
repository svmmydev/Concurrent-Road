
using System.Net.Sockets;
using CarreteraClass;
using Infraestructura.Utils;
using NetworkStreamNS;
using PuenteClass;
using VehiculoClass;

namespace Servidor;

public class RecepcionVehiculoHandler
{
    private static object broadcastLock = new object();


    /// <summary>
    /// Gestiona el ciclo de vida de un vehículo desde su incorporación a la carretera hasta su finalización.
    /// Controla su paso por el puente, actualiza su estado en la carretera y sincroniza la información con todos los clientes.
    /// Si el vehículo se desconecta inesperadamente, libera el puente y lo marca como desconectado.
    /// </summary>
    /// <param name="cliente">Cliente que representa al vehículo.</param>
    /// <param name="carretera">Instancia compartida de la carretera para actualizar y compartir el estado del vehículo.</param>
    /// <returns>Una tarea asincrónica que representa la gestión completa del vehículo.</returns>
    public static async Task GestionarVehiculoAsync(Cliente cliente, Carretera carretera)
    {
        Vehiculo vehiculo = await cliente.ClienteNetwS.LeerDatosVehiculoNSAsync();
        carretera.AñadirVehiculo(vehiculo);

        bool enPuente = false;

        try
        {
            while (!vehiculo.Acabado)
            {
                vehiculo = await cliente.ClienteNetwS.LeerDatosVehiculoNSAsync();

                if (!enPuente && vehiculo.Pos == 39)
                {
                    vehiculo.Parado = true;
                    carretera.ActualizarVehiculo(vehiculo);
                    EnviarEstadoCarretera(carretera);

                    await Puente.EntrarPuenteAsync(vehiculo);
                    enPuente = true;

                    vehiculo.Parado = false;
                }

                if (enPuente && vehiculo.Pos == 61)
                {
                    Puente.SalirPuente(vehiculo.Id);
                    enPuente = false;
                }

                vehiculo.Desconectado = false;
                carretera.ActualizarVehiculo(vehiculo);
                EnviarEstadoCarretera(carretera);
            }

            Consola.Success($"El cliente con id #{cliente.ClienteId} ha realizado el servicio con éxito y se le ha desconectado del servidor");
        }
        catch (Exception ex)
        {
            vehiculo.Desconectado = true;
            Consola.Error($"Error con el vehículo {vehiculo.Id}: {ex.Message}");
        }
        finally
        {
            carretera.ActualizarVehiculo(vehiculo);
            EnviarEstadoCarretera(carretera);
                
            if (enPuente)
            {
                Puente.SalirPuente(vehiculo.Id);
                ClienteManager.EliminarCliente(cliente.ClienteId);

                Consola.Error($"Puente liberado forzosamente por caída del vehículo {vehiculo.Id}");
            }
        }
    }



    /// <summary>
    /// Envía el estado actual de la carretera a todos los clientes conectados.
    /// Si un cliente falla, se elimina de la lista de clientes activos.
    /// </summary>
    /// <param name="carretera">Carretera actual que se desea compartir con los clientes.</param>
    public static void EnviarEstadoCarretera(Carretera carretera)
    {
        lock(broadcastLock)
        {
            foreach (Cliente cliente in ClienteManager.ClientesConectados())
            {
                try
                {
                    cliente.ClienteNetwS.EscribirDatosCarreteraNS(carretera);
                }
                catch
                {
                    ClienteManager.EliminarCliente(cliente.ClienteId);
                }
            }
        }
    }
}