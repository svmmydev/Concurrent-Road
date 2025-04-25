using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;
using System.Threading.Tasks;

namespace Servidor
{
    class Program
    {
        static TcpListener? Servidor;
        private static int IdUnico = 0;


        static async Task Main(string[] args)
        {            
            byte[] bufferLectura = new byte[1024];

            Servidor = new TcpListener(IPAddress.Parse("127.0.0.1"), 10001);
            Servidor.Start();
            Console.WriteLine("Servidor: Servidor iniciado");
            Console.WriteLine("Servidor: Esperando vehículos..");

            while (true)
            {
                TcpClient Cliente = await Servidor.AcceptTcpClientAsync();
                _ = GestionarClienteAsync(Cliente);
            }
        }


        private static async Task GestionarClienteAsync(TcpClient cliente)
        {
            int clienteId = Interlocked.Increment(ref IdUnico);
            Console.WriteLine($"\nServidor: Gestionando nuevo vehículo #{clienteId}");

            NetworkStream netwS = cliente.GetStream();

            try
            {
                string inicio = await netwS.LeerMensajeAsync();
                if (inicio != "INICIO")
                {
                    Console.WriteLine($"# Error: Handshake iniciado incorrecto: {inicio}");
                    cliente.Close();
                    return;
                }

                await netwS.EscribirMensajeAsync(clienteId.ToString());

                string confirmacion = await netwS.LeerMensajeAsync();
                if (confirmacion != clienteId.ToString())
                {
                    Console.WriteLine($"# Error: Confirmación de ID incorrecta: {confirmacion}");
                    cliente.Close();
                    return;
                }

                Console.WriteLine($"Handshake OK con vehículo #{clienteId}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error: Conexión con el cliente {clienteId} fallida: {e.Message}");
            }
        }
    }
}

