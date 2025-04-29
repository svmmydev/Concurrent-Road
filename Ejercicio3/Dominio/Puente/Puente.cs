using System.Threading;
using VehiculoClass;

namespace PuenteClass;

public class Puente
{
    private readonly SemaphoreSlim semaforoPuente = new SemaphoreSlim(1, 1);

    public async Task EntrarPuenteAsync(Vehiculo vehiculo)
    {
        await semaforoPuente.WaitAsync();
    }

    public void SalirPuente(Vehiculo vehiculo)
    {
        semaforoPuente.Release();
    }
}
