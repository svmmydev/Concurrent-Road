
using System.Net.Sockets;
using NetworkStreamNS;

namespace Servidor.Handlers
{
    public static class HandshakeHandler
    {
        
        private static int IdUnico = 0;

        public static async Task GestionarClienteAsync(TcpClient cliente)
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

                Cliente clienteNuevo = new Cliente(clienteId, netwS);
                ClienteManager.AñadirCliente(clienteNuevo);

                Console.WriteLine($"Handshake OK con vehículo #{clienteId}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error: Conexión con el cliente {clienteId} fallida: {e.Message}");
            }
        }
    }
}