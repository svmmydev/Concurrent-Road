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
        // Escribe un objeto Carretera con prefijo de longitud
        public static void EscribirDatosCarreteraNS(this NetworkStream netwS, Carretera C)
        {
            byte[] data = C.CarreteraABytes();
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));
            netwS.Write(header, 0, header.Length);
            netwS.Write(data, 0, data.Length);
        }

        // Lee un objeto Carretera leyendo primero el prefijo de longitud
        public static Carretera LeerDatosCarreteraNS(this NetworkStream netwS)
        {
            byte[] header = new byte[4];
            int read = netwS.Read(header, 0, 4);
            if (read < 4)
                throw new IOException("Conexión cerrada al leer longitud de Carretera");

            int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(header, 0));
            byte[] buf = new byte[length];
            int total = 0;
            while (total < length)
            {
                int n = netwS.Read(buf, total, length - total);
                if (n == 0)
                    throw new IOException("Conexión cerrada al leer Carretera");
                total += n;
            }

            return Carretera.BytesACarretera(buf);
        }

        // Escribe un objeto Vehiculo con prefijo de longitud
        public static void EscribirDatosVehiculoNS(this NetworkStream netwS, Vehiculo V)
        {
            byte[] data = V.VehiculoaBytes();
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length));
            netwS.Write(header, 0, header.Length);
            netwS.Write(data, 0, data.Length);
        }

        // Lee un objeto Vehiculo leyendo primero el prefijo de longitud
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

        // Métodos de mensajes de texto existentes (sin cambios)
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

        public static async Task EscribirMensajeAsync(this NetworkStream netwS, string Str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(Str);
            await netwS.WriteAsync(buffer.AsMemory());
        }
    }
}
