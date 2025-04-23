using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using VehiculoClass;
using CarreteraClass;


namespace NetworkStreamNS
{
    public class NetworkStreamClass
    {
        
        //Método para escribir en un NetworkStream los datos de tipo Carretera
        public static void  EscribirDatosCarreteraNS(NetworkStream NS, Carretera C)
        {            
                            
        }

        //Metódo para leer de un NetworkStream los datos que de un objeto Carretera
        /*public static Carretera LeerDatosCarreteraNS (NetworkStream NS)
        {
            

        }*/

        //Método para enviar datos de tipo Vehiculo en un NetworkStream
        public static void  EscribirDatosVehiculoNS(NetworkStream NS, Vehiculo V)
        {            
                              
        }

        //Metódo para leer de un NetworkStream los datos que de un objeto Vehiculo
        /*public static Vehiculo LeerDatosVehiculoNS (NetworkStream NS)
        {

        }*/


        private class State
        {
            public NetworkStream NetwStream {get;set;}
            public byte[] BufferLectura = new byte[1024];
            public MemoryStream MemStream = new MemoryStream();
            public Action<string> OnMessage;
        }


        //Método que permite leer un mensaje de tipo texto (string) de un NetworkStream
        public static void LeerMensajeNetworkStream(NetworkStream NS, Action<string> onMessage)
        {
            State state = new State();
            state.NetwStream = NS;
            state.OnMessage = onMessage;

            NS.BeginRead(state.BufferLectura, 0, state.BufferLectura.Length, EndRead, state);
        }


        private static void EndRead(IAsyncResult ar)
        {
            State state = (State)ar.AsyncState;
            int bytesLeidos;

            try
            {
                bytesLeidos = state.NetwStream.EndRead(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (IOException)
            {
                LimpiarConexion(state);
                return;
            }

            if (bytesLeidos == 0)
            {
                LimpiarConexion(state);
                return;
            }

            state.MemStream.Write(state.BufferLectura, 0, bytesLeidos);

            if (state.NetwStream.DataAvailable)
            {
                state.NetwStream.BeginRead(state.BufferLectura, 0, state.BufferLectura.Length, EndRead, state);
            }
            else
            {
                string mensaje = Encoding.UTF8.GetString(state.MemStream.ToArray(), 0, (int)state.MemStream.Length);
                state.MemStream.SetLength(0);

                state.OnMessage(mensaje);

                state.NetwStream.BeginRead(state.BufferLectura, 0, state.BufferLectura.Length, EndRead, state);
            }
        }


        //Método que permite escribir un mensaje de tipo texto (string) al NetworkStream
        public static void EscribirMensajeNetworkStream(NetworkStream NS, string Str, Action onWritten)
        {            
            byte[] MensajeBytes = Encoding.UTF8.GetBytes(Str);
            NS.BeginWrite(MensajeBytes, 0, MensajeBytes.Length, EndWrite, (NS, onWritten));
        }


        private static void EndWrite(IAsyncResult ar)
        {
            NetworkStream NS;
            Action onWritten;
            try
            {
                (NS, onWritten) = ((NetworkStream,Action))ar.AsyncState;
                NS.EndWrite(ar);
            }
            catch
            {
                return;
            }
            onWritten();
        }


        static void LimpiarConexion(State state)
        {
            try { state.NetwStream.Close(); } catch { }
            try { state.MemStream.Dispose(); } catch { }
        }
    }
}
