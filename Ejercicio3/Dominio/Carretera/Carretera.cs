using System.Xml.Serialization;
using VehiculoClass;

namespace CarreteraClass;

[Serializable]
public class Carretera
{
    public List<Vehiculo> VehiculosEnCarretera = new List<Vehiculo>();
    public int NumVehiculosEnCarrera = 0;


    /// <summary>
    /// Constructor por defecto necesario para la serialización XML.
    /// </summary>
    public Carretera ()
    {

    }


    /// <summary>
    /// Crea y añade un nuevo vehículo vacío a la lista de vehículos en carretera.
    /// </summary>
    public void CrearVehiculo ()
    {
        Vehiculo V = new Vehiculo();
        VehiculosEnCarretera.Add(V);
    }


    /// <summary>
    /// Añade un vehículo ya creado a la lista de vehículos en carretera y aumenta el contador total.
    /// </summary>
    /// <param name="V">Vehículo a añadir.</param>
    public void AñadirVehiculo (Vehiculo V)
    {
        VehiculosEnCarretera.Add(V);
        NumVehiculosEnCarrera++;
    }


    /// <summary>
    /// Actualiza los datos (posición, velocidad y estado) de un vehículo existente en la carretera.
    /// </summary>
    /// <param name="V">Vehículo con los datos actualizados.</param>
    public void ActualizarVehiculo (Vehiculo V)
    {
        Vehiculo veh = VehiculosEnCarretera.FirstOrDefault(x => x.Id == V.Id);
        if (veh != null) 
        {
            veh.Pos = V.Pos;
            veh.Velocidad = V.Velocidad;
            veh.Parado = V.Parado;
            veh.Desconectado = V.Desconectado;
        }
    }


    /// <summary>
    /// Muestra por consola las posiciones de todos los vehículos actualmente en la carretera.
    /// </summary>
    public void MostrarVehiculos ()
    {
        string strVehs = "";
        foreach (Vehiculo v in VehiculosEnCarretera)
        {
            strVehs = strVehs + "\t" + v.Pos;
        }

        Console.WriteLine(strVehs);
    }


    /// <summary>
    /// Serializa el objeto Carretera en un array de bytes usando formato XML.
    /// </summary>
    /// <returns>Array de bytes que representa el objeto serializado.</returns>
    public byte[] CarreteraABytes()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Carretera));
            
        MemoryStream MS = new MemoryStream();
  
        serializer.Serialize(MS, this);
       
        return MS.ToArray();
    }


    /// <summary>
    /// Deserializa un array de bytes en un objeto Carretera.
    /// </summary>
    /// <param name="bytesCarrera">Array de bytes que contiene los datos serializados.</param>
    /// <returns>Objeto Carretera reconstruido.</returns>
    public static Carretera BytesACarretera(byte[] bytesCarrera)
    {
        Carretera tmpCarretera; 
        
        XmlSerializer serializer = new XmlSerializer(typeof(Carretera));

        MemoryStream MS = new MemoryStream(bytesCarrera);

        tmpCarretera = (Carretera) serializer.Deserialize(MS);

        return tmpCarretera;
    }    
}