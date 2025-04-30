using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using VehiculoClass;
using CarreteraClass;

namespace NetworkStreamNS
{
    public static class NetworkStreamClass
    {
        /// <summary>
        /// Escribe un objeto Carretera en el NetworkStream incluyendo una cabecera con su tamaño.
        /// </summary>
        /// <param name="netwS">Stream de red donde se escriben los datos.</param>
        /// <param name="C">Objeto Carretera a serializar y enviar.</param>
        public static void EscribirDatosCarreteraNS(this NetworkStream netwS, Carretera C)
        {
            byte[] data = C.CarreteraABytes();
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));
            netwS.Write(header, 0, header.Length);
            netwS.Write(data, 0, data.Length);
        }


        /// <summary>
        /// Lee un objeto Carretera desde el NetworkStream, leyendo primero la cabecera de longitud.
        /// </summary>
        /// <param name="netwS">Stream de red desde el que se leen los datos.</param>
        /// <returns>Objeto Carretera deserializado.</returns>
        /// <exception IOException>Si la conexión se cierra inesperadamente.</exception>
        public static Carretera LeerDatosCarreteraNS(this NetworkStream netwS)
        {
            byte[] header = new byte[4];
            int read = netwS.Read(header, 0, 4);
            if (read < 4) throw new IOException("Conexión cerrada al leer longitud de Carretera");

            int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(header, 0));
            byte[] buf = new byte[length];
            int total = 0;
            while (total < length)
            {
                int n = netwS.Read(buf, total, length - total);
                if (n == 0) throw new IOException("Conexión cerrada al leer Carretera");
                total += n;
            }

            return Carretera.BytesACarretera(buf);
        }


        /// <summary>
        /// Serializa y envía un objeto Vehiculo por el NetworkStream, incluyendo una cabecera de longitud.
        /// </summary>
        /// <param name="netwS">Stream de red por donde se envía el objeto.</param>
        /// <param name="V">Vehículo a serializar y enviar.</param>
        /// <returns>Una tarea asincrónica que representa la operación de escritura.</returns>
        public static async Task EscribirDatosVehiculoNSAsync(this NetworkStream netwS, Vehiculo V)
        {
            byte[] data = V.VehiculoaBytes();
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));
            await netwS.WriteAsync(header, 0, header.Length);
            await netwS.WriteAsync(data, 0, data.Length);
        }


        /// <summary>
        /// Recibe y deserializa un objeto Vehiculo desde el NetworkStream, interpretando su longitud primero.
        /// </summary>
        /// <param name="netwS">Stream de red desde donde se recibe el objeto.</param>
        /// <returns>Vehículo reconstruido a partir del stream.</returns>
        /// <exception IOException>Si la conexión se cierra inesperadamente.</exception>
        public static async Task<Vehiculo> LeerDatosVehiculoNSAsync(this NetworkStream netwS)
        {
            byte[] header = new byte[4];
            int read = await netwS.ReadAsync(header, 0, 4);
            if (read < 4) throw new IOException("Conexión cerrada al leer longitud de Vehículo");

            int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(header, 0));
            byte[] buf = new byte[length];
            int total = 0;
            while (total < length)
            {
                int n = await netwS.ReadAsync(buf, total, length - total);
                if (n == 0) throw new IOException("Conexión cerrada al leer Vehículo");
                total += n;
            }

            return Vehiculo.BytesAVehiculo(buf);
        }


        /// <summary>
        /// Lee un mensaje de texto completo desde el NetworkStream hasta que no haya más datos disponibles.
        /// </summary>
        /// <param name="netwS">Stream de red desde el que se lee el mensaje.</param>
        /// <returns>Mensaje recibido como string.</returns>
        /// <exception IOException>Si la conexión se cierra inesperadamente.</exception>
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


        /// <summary>
        /// Escribe un mensaje de texto como string en el NetworkStream, codificado en UTF-8.
        /// </summary>
        /// <param name="netwS">Stream de red donde se envía el mensaje.</param>
        /// <param name="Str">Cadena de texto a enviar.</param>
        /// <returns>Una tarea asincrónica que representa la operación de escritura.</returns>
        public static async Task EscribirMensajeAsync(this NetworkStream netwS, string Str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(Str);
            await netwS.WriteAsync(buffer.AsMemory());
        }
    }
}
