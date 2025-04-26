
using System.Net.Sockets;
using System.Net;
using Servidor.Handlers;

namespace Servidor;

class Program
{
    static TcpListener? Servidor;


    static async Task Main(string[] args)
    {
        Servidor = new TcpListener(IPAddress.Parse("127.0.0.1"), 10001);
        Servidor.Start();

        Console.WriteLine("Servidor: Servidor iniciado");
        Console.WriteLine("Servidor: Esperando vehículos..");

        while (true)
        {
            TcpClient Cliente = await Servidor.AcceptTcpClientAsync();
            _ = HandshakeHandler.GestionarClienteAsync(Cliente);
        }
    }
}

