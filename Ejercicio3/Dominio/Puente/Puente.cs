using System.Threading;
using VehiculoClass;

namespace PuenteClass;

public class Puente
{
    private readonly SemaphoreSlim semaforoPuente = new SemaphoreSlim(1, 1);

    public async Task EntrarPuenteAsync(Vehiculo vehiculo)
    {
        Console.WriteLine($"Vehículo {vehiculo.Id} esperando para cruzar el puente...");
        await semaforoPuente.WaitAsync();
        Console.WriteLine($"Vehículo {vehiculo.Id} ha entrado al puente.");
    }

    public void SalirPuente(Vehiculo vehiculo)
    {
        Console.WriteLine($"Vehículo {vehiculo.Id} ha salido del puente.");
        semaforoPuente.Release();
    }
}
