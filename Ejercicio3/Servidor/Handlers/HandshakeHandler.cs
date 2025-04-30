
using System.Net.Sockets;
using CarreteraClass;
using Infraestructura.Utils;
using NetworkStreamNS;

namespace Servidor.Handlers;

public static class HandshakeHandler
{
    private static int IdUnico = 0;
    
    
    /// <summary>
    /// Gestiona la conexión de un nuevo cliente, realiza el handshake inicial, verifica el ID y lanza las tareas necesarias.
    /// </summary>
    /// <param name="cliente">Cliente TCP conectado al servidor.</param>
    /// <param name="carretera">Instancia compartida de la carretera para sincronizar el estado del vehículo.</param>
    /// <returns>Una tarea asincrónica que gestiona el ciclo de vida inicial del cliente.</returns>
    public static async Task GestionarClienteAsync(TcpClient cliente, Carretera carretera)
    {
        int clienteId = Interlocked.Increment(ref IdUnico);
        Consola.Info($"Gestionando nuevo vehículo #{clienteId}");

        NetworkStream netwS = cliente.GetStream();

        try
        {
            string inicio = await netwS.LeerMensajeAsync();
            if (inicio != "INICIO")
            {
                Consola.Error($"Handshake iniciado incorrecto: {inicio}");
                cliente.Close();
                
                return;
            }

            await netwS.EscribirMensajeAsync(clienteId.ToString());

            string confirmacion = await netwS.LeerMensajeAsync();
            if (confirmacion != clienteId.ToString())
            {
                Consola.Error($"Confirmación de ID incorrecta: {confirmacion}");
                cliente.Close();

                return;
            }

            Consola.Success($"Handshake OK con vehículo #{clienteId}");

            ClienteManager.GestionarCliente(clienteId, netwS);

            _ = Task.Run(() => RecepcionVehiculoHandler.GestionarVehiculoAsync(netwS, carretera));
            
            ClienteManager.MostrarClientesConectados();
        }
        catch (Exception e)
        {
            Consola.Error($"Conexión con el cliente {clienteId} fallida: {e.Message}");
        }
    }
}