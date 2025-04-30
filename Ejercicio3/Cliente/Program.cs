
using System.Net.Sockets;
using Cliente.Handlers;
using Infraestructura.Utils;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente
{
    class Program
    {
        /// <summary>
        /// Punto de entrada principal del cliente. Establece conexión con el servidor,
        /// realiza el handshake inicial, registra el vehículo, lanza el hilo para actualizar
        /// la carretera y comienza el movimiento del vehículo.
        /// </summary>
        static async Task Main(string[] args)
        {
            try
            {
                TcpClient Cliente = new TcpClient();
                await Cliente.ConnectAsync("127.0.0.1", 10001);

                NetworkStream netwS = Cliente.GetStream();

                string id = await ClienteHandshake.InicioHandshakeCliente(netwS);

                Vehiculo vehiculo = await VehiculoHandler.IniciarVehiculoAsync(netwS, int.Parse(id));

                _ = Task.Run(() => CarreteraHandler.ActualizarCarretera(netwS));

                await VehiculoHandler.MoverVehiculoAsync(netwS);
            }
            catch (Exception e)
            {
                Consola.Error($"Error al conectar con el servidor: {e.Message}");
            }
        }
    }
}