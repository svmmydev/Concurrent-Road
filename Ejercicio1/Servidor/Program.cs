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

            Servidor.BeginAcceptTcpClient(EndAccept, Servidor);

            Console.WriteLine("Servidor: Esperando clientes..");

            Console.ReadLine();
        }


        static void EndAccept(IAsyncResult ar)
        {
            TcpListener Servidor = (TcpListener)ar.AsyncState!;
            TcpClient Cliente = Servidor.EndAcceptTcpClient(ar);

            Servidor.BeginAcceptTcpClient(EndAccept, Servidor);

            NetworkStream FlujoDatos = Cliente.GetStream();

            int clienteId = Interlocked.Increment(ref IdUnico);

            NetworkStreamClass.LeerMensajeNetworkStream(FlujoDatos, inicio => {
                if (inicio != "INICIO") {
                    Console.WriteLine("Error al establecer la conexión: Inicio handshake fallido");
                    Cliente.Close();
                    return;
                }

                NetworkStreamClass.EscribirMensajeNetworkStream(FlujoDatos, clienteId.ToString(), () => {

                    NetworkStreamClass.LeerMensajeNetworkStream(FlujoDatos, confirm => {
                        if (confirm != clienteId.ToString())
                        {
                            Console.WriteLine("Error al establecer la conexión: ID incorrecto");
                            Cliente.Close();
                            return;
                        }
                        
                        Console.WriteLine($"Handshake OK con vehículo {clienteId}");
                    });
                });
            });
            
            Console.WriteLine($"\nServidor: Gestionando nuevo vehículo.. {IdUnico}");
        }
    }
}

