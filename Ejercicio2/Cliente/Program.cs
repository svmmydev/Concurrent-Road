
using System.Net.Sockets;

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

                VehiculoManager.IniciarVehiculo(netwS, int.Parse(id));
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error al conectar con el servidor: {e.Message}");
            }

            Console.WriteLine("Presiona Enter para salir..");
            Console.ReadLine();
        }
    }
}