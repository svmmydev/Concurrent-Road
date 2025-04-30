
using System.Net.Sockets;
using System.Net;
using Servidor.Handlers;
using CarreteraClass;
using Infraestructura.Utils;

namespace Servidor;

class Program
{
    static TcpListener? Servidor;
    static Carretera carretera = new Carretera();


    /// <summary>
    /// Punto de entrada principal del servidor. Inicia un listener TCP, acepta conexiones entrantes
    /// y delega su gestión al manejador de handshakes.
    /// </summary>
    /// <param name="args">Argumentos de línea de comandos (no utilizados).</param>
    /// <returns>Una tarea asincrónica que representa la ejecución del servidor.</returns>
    static async Task Main(string[] args)
    {
        Servidor = new TcpListener(IPAddress.Parse("127.0.0.1"), 10001);
        Servidor.Start();

        Consola.Info("Servidor iniciado, esperando clientes..");

        while (true)
        {
            TcpClient Cliente = await Servidor.AcceptTcpClientAsync();
            _ = HandshakeHandler.GestionarClienteAsync(Cliente, carretera);
        }
    }
}

