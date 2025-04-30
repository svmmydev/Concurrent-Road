
using System;

namespace Infraestructura.Utils;

public static class Consola
{
    /// <summary>
    /// Escribe un mensaje informativo en consola con color cian.
    /// </summary>
    /// <param name="mensaje">Contenido del mensaje a mostrar.</param>
    public static void Info(string mensaje)
    {
        Escribir("[INFO]", mensaje, ConsoleColor.Cyan);
    }


    /// <summary>
    /// Escribe un mensaje de éxito en consola con color verde.
    /// </summary>
    /// <param name="mensaje">Contenido del mensaje a mostrar.</param>
    public static void Success(string mensaje)
    {
        Escribir("[SUCCESS]", mensaje, ConsoleColor.Green);
    }


    /// <summary>
    /// Escribe un mensaje de error en consola con color rojo.
    /// </summary>
    /// <param name="mensaje">Contenido del mensaje a mostrar.</param>
    public static void Error(string mensaje)
    {
        Escribir("[ERROR]", mensaje, ConsoleColor.Red);
    }

    
    /// <summary>
    /// Escribe un mensaje de advertencia en consola con color amarillo.
    /// </summary>
    /// <param name="mensaje">Contenido del mensaje a mostrar.</param>
    public static void Warn(string mensaje)
    {
        Escribir("[WARN]", mensaje, ConsoleColor.Yellow);
    }


    /// <summary>
    /// Método interno que formatea y muestra un mensaje en consola con el nivel, timestamp y color especificado.
    /// </summary>
    /// <param name="nivel">Etiqueta del tipo de mensaje (INFO, ERROR, etc.).</param>
    /// <param name="mensaje">Mensaje a mostrar.</param>
    /// <param name="color">Color con el que se mostrará el mensaje en consola.</param>
    private static void Escribir(string nivel, string mensaje, ConsoleColor color)
    {
        var timestamp = DateTime.Now.ToString("\nHH:mm:ss");
        Console.ForegroundColor = color;
        Console.WriteLine($"{timestamp} [SERVER] {nivel} {mensaje}.");
        Console.ResetColor();
    }
}