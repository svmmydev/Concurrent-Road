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
        // Método para escribir en un NetworkStream los datos de tipo Carretera
        public static void EscribirDatosCarreteraNS(this NetworkStream netwS, Carretera C)
        {
            byte[] data = C.CarreteraABytes();
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));
            netwS.Write(header, 0, header.Length);
            netwS.Write(data, 0, data.Length);
        }

        // Método para leer de un NetworkStream los datos que de un objeto Carretera
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

        // Método para enviar datos de tipo Vehiculo en un NetworkStream
        public static void EscribirDatosVehiculoNS(this NetworkStream netwS, Vehiculo V)
        {
            byte[] data = V.VehiculoaBytes();
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));
            netwS.Write(header, 0, header.Length);
            netwS.Write(data, 0, data.Length);
        }

        // Método para leer de un NetworkStream los datos que de un objeto Vehiculo
        public static Vehiculo LeerDatosVehiculoNS(this NetworkStream netwS)
        {
            byte[] header = new byte[4];
            int read = netwS.Read(header, 0, 4);
            if (read < 4)
                throw new IOException("Conexión cerrada al leer longitud de Vehículo");

            int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(header, 0));
            byte[] buf = new byte[length];
            int total = 0;
            while (total < length)
            {
                int n = netwS.Read(buf, total, length - total);
                if (n == 0)
                    throw new IOException("Conexión cerrada al leer Vehículo");
                total += n;
            }

            return Vehiculo.BytesAVehiculo(buf);
        }

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
