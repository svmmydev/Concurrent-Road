using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Client
{
    class Program
    {
        static TcpClient? Cliente;

        static void Main(string[] args)
        {
            Cliente = new TcpClient();
            Cliente.Connect("127.0.0.1", 10001);

            try
            {
                if (Cliente.Connected)
                {
                    Console.WriteLine("Cliente: Cliente conectado");

                    NetworkStream FlujoDatos = Cliente.GetStream();

                    NetworkStreamClass.EscribirMensajeNetworkStream(FlujoDatos, "INICIO", () => {

                        NetworkStreamClass.LeerMensajeNetworkStream(FlujoDatos, clienteId => {
                            Console.WriteLine($"ID asignado: {clienteId}");

                            NetworkStreamClass.EscribirMensajeNetworkStream(FlujoDatos, clienteId, () => {
                                Console.WriteLine("Handshake completado!");
                            });
                        });
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al conectar con el servidor:");
            }

            Console.ReadLine();
        }
    }
}