﻿using System.Xml.Serialization;
using VehiculoClass;

namespace CarreteraClass;

[Serializable]
public class Carretera
{
    public List<Vehiculo> VehiculosEnCarretera = new List<Vehiculo>();
    public int NumVehiculosEnCarrera = 0;
    const int totalKm = 100;
    const int tamañoBarra = 20;
    static object consoleLock = new object();


    public Carretera ()
    {

    }


    //Crea un nuevo vehiculo
    public void CrearVehiculo ()
    {
        Vehiculo V = new Vehiculo();
        VehiculosEnCarretera.Add(V);
    }


    //Añade un vehiculo ya creado a la lista de vehiculos en carretera
    public void AñadirVehiculo (Vehiculo V)
    {
        VehiculosEnCarretera.Add(V);
        NumVehiculosEnCarrera++;
    }


    //Actualiza los datos de un vehiculo ya existente en la lista de vehiculos en carretera. 
    public void ActualizarVehiculo (Vehiculo V)
    {
        Vehiculo veh = VehiculosEnCarretera.FirstOrDefault(x => x.Id == V.Id);
        if (veh != null) 
        {
            veh.Pos = V.Pos;
            veh.Velocidad = V.Velocidad;
        }
    }


    //Muestra por pantalla los vehiculos en carretera. 
    public void MostrarCarretera ()
    {
        lock(consoleLock)
        {
            Console.Clear();

            foreach (Vehiculo vehiculo in VehiculosEnCarretera.OrderBy(x => x.Id))
            {
                double porcentaje = (double)vehiculo.Pos / totalKm;
                int llenos = (int)(porcentaje * tamañoBarra);
                int vacios = tamañoBarra - llenos;

                string carretera = new string('█', llenos) + new string('▒', vacios);

                string estado = vehiculo.Pos >= totalKm ? "Terminado"
                                                        : $"{vehiculo.Pos} km";

                Console.WriteLine($"Vehículo #{vehiculo.Id}: {carretera} ({estado}) [{vehiculo.Direccion}]");
            }
        }
    }


    //Permite serializar Carretera a array de bytes mediant formato XML
    public byte[] CarreteraABytes()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Carretera));
            
        MemoryStream MS = new MemoryStream();
  
        serializer.Serialize(MS, this);
       
        return MS.ToArray();
    }


    //Permite desserializar una cadena de bytes a un objeto de tipo Carretera
    public static Carretera BytesACarretera(byte[] bytesCarrera)
    {
        Carretera tmpCarretera; 
        
        XmlSerializer serializer = new XmlSerializer(typeof(Carretera));

        MemoryStream MS = new MemoryStream(bytesCarrera);

        tmpCarretera = (Carretera) serializer.Deserialize(MS);

        return tmpCarretera;
    }    
}
