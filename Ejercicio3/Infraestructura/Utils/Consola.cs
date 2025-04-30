
using System;

namespace Infraestructura.Utils;

public static class Consola
{
    public const string Server = "[Server]";


    public static void Info(string mensaje)
    {
        Escribir("[INFO]", mensaje, ConsoleColor.Cyan);
    }


    public static void Success(string mensaje)
    {
        Escribir("[SUCCESS]", mensaje, ConsoleColor.Green);
    }


    public static void Error(string mensaje)
    {
        Escribir("[ERROR]", mensaje, ConsoleColor.Red);
    }

    
    public static void Warn(string mensaje)
    {
        Escribir("[WARN]", mensaje, ConsoleColor.Yellow);
    }


    private static void Escribir(string nivel, string mensaje, ConsoleColor color)
    {
        var timestamp = DateTime.Now.ToString("\nHH:mm:ss");
        Console.ForegroundColor = color;
        Console.WriteLine($"{timestamp} [SERVER] {nivel} {mensaje}.");
        Console.ResetColor();
    }
}