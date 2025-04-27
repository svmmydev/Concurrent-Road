
using System.Net.Sockets;
using Cliente.Handlers;
using NetworkStreamNS;
using VehiculoClass;

namespace Cliente
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                TcpClient Cliente = new TcpClient();
                await Cliente.ConnectAsync("127.0.0.1", 10001);
                
                Console.WriteLine("Cliente: Conectado al servidor");

                NetworkStream netwS = Cliente.GetStream();

                string id = await ClienteHandshake.InicioHandshakeCliente(netwS);

                Vehiculo vehiculo = VehiculoHandler.IniciarVehiculo(netwS, int.Parse(id));
                Console.WriteLine($"Vehículo iniciado con ID #{id} y velocidad {vehiculo.Velocidad}");

                _ = Task.Run(() => CarreteraHandler.ActualizarCarretera(netwS));

                VehiculoHandler.MoverVehiculo(netwS);
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error al conectar con el servidor: {e.Message}");
            }
        }
    }
}