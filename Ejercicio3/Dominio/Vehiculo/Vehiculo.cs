using System.Xml.Serialization;

namespace VehiculoClass;

[Serializable]
public class Vehiculo
{
    public int Id {get; set;}
    public int Pos {get;set;}
    public int Velocidad {get; set;}
    public string Direccion {get; set;} // "Norte" o "Sur" 
    public bool Acabado {get;set;}
    public bool Parado {get; set;}
    
    
    /// <summary>
    /// Constructor por defecto que inicializa el vehículo con una velocidad aleatoria,
    /// posición inicial en 0 y dirección aleatoria ("Norte" o "Sur").
    /// </summary>
    public Vehiculo()
    {
        var random = new Random();

        Velocidad = random.Next(100,500);
        Pos = 0;
        Acabado = false;
        Direccion = (random.Next(0, 2) == 1) ? "Norte" : "Sur";
    }


    /// <summary>
    /// Serializa el objeto Vehiculo en un array de bytes utilizando formato XML.
    /// </summary>
    /// <returns>Array de bytes que representa el objeto serializado.</returns>
    public byte[] VehiculoaBytes()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Vehiculo));
            
        MemoryStream MS = new MemoryStream();
  
        serializer.Serialize(MS, this);
       
        return MS.ToArray();
    }


    /// <summary>
    /// Deserializa un array de bytes en un objeto Vehiculo.
    /// </summary>
    /// <param name="bytesVehiculo">Array de bytes que contiene los datos serializados del vehículo.</param>
    /// <returns>Objeto Vehiculo reconstruido desde los datos.</returns>
    public static Vehiculo BytesAVehiculo(byte[] bytesVehiculo)
    {
        Vehiculo tmpVehiculo; 
        
        XmlSerializer serializer = new XmlSerializer(typeof(Vehiculo));

        MemoryStream MS = new MemoryStream(bytesVehiculo);

        tmpVehiculo = (Vehiculo)serializer.Deserialize(MS);

        return tmpVehiculo;
    }
}
