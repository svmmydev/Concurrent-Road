using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Servidor
{
    class Program
    {
        static TcpListener? Servidor;
        private static int IdUnico = 0;


        static void Main(string[] args)
        {            
            byte[] bufferLectura = new byte[1024];

            Servidor = new TcpListener(IPAddress.Parse("127.0.0.1"), 10001);
            Servidor.Start();
            Console.WriteLine("Servidor: Servidor iniciado");

            // TO:DO Aceptar conexión clientes
        }


        static void EndAccept(IAsyncResult ar)
        {
            TcpListener Servidor = (TcpListener)ar.AsyncState!;
            TcpClient Cliente = Servidor.EndAcceptTcpClient(ar);

            int clienteId = Interlocked.Increment(ref IdUnico);
            NetworkStream FlujoDatos = Cliente.GetStream();

            // TO:DO Conexión y Handshake

            Console.WriteLine($"\nServidor: Gestionando nuevo vehículo.. {IdUnico}");
        }
    }
}

