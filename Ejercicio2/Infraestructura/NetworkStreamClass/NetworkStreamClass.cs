using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using VehiculoClass;
using CarreteraClass;
using System.Threading.Tasks;


namespace NetworkStreamNS
{
    public static class NetworkStreamClass
    {
        
        // Método para escribir en un NetworkStream los datos de tipo Carretera
        public static void EscribirDatosCarreteraNS(this NetworkStream netwS, Carretera C)
        {            
                            
        }

        // Metódo para leer de un NetworkStream los datos que de un objeto Carretera
        /*public static Carretera LeerDatosCarreteraNS (this NetworkStream netwS)
        {
            

        }*/

        // Método para enviar datos de tipo Vehiculo en un NetworkStream
        public static void EscribirDatosVehiculoNS(this NetworkStream netwS, Vehiculo V)
        {            
                              
        }

        // Metódo para leer de un NetworkStream los datos que de un objeto Vehiculo
        /*public static Vehiculo LeerDatosVehiculoNS (this NetworkStream netwS)
        {

        }*/


        // Método que permite leer un mensaje de tipo texto (string) de un NetworkStream
        public static async Task<string> LeerMensajeAsync(this NetworkStream netwS)
        {
            using var memS = new MemoryStream();
            byte[] buffer = new byte[1024];
            int bytesLeidos;

            do
            {
                bytesLeidos = await netwS.ReadAsync(buffer.AsMemory());

                if (bytesLeidos == 0) throw new IOException("Conexión cerrada insesperadamente");

                memS.Write(buffer, 0, bytesLeidos);
            }
            while (netwS.DataAvailable);

            return Encoding.UTF8.GetString(memS.ToArray());
        }


        // Método que permite escribir un mensaje de tipo texto (string) al NetworkStream
        public static async Task EscribirMensajeAsync(this NetworkStream netwS, string Str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(Str);
            await netwS.WriteAsync(buffer.AsMemory());
        }
    }
}
