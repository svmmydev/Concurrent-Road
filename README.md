<div align="center">

# SIMULACIÓN DE TRÁFICO EN UNA CARRETERA CONCURRENTE

</div>

<br>

La simulación representa una carretera donde múltiples vehículos (clientes) circulan y acceden a un puente estrecho que solo permite el paso de uno en uno. Los vehículos se comunican con un servidor que mantiene el estado global de la carretera. La entrada y salida al puente está controlada por un semáforo y prioridad, garantizando la sincronización entre los hilos. La interfaz de consola permite visualizar en tiempo real el avance de los vehículos, así como desconexiones inesperadas.

<br>

<div align="center">

![Simulación de carretera](Assets/Images/10-mostrar-carretera-en-cliente.png)

</div>

<br>

## Índice

- [Características](#características)
- [Arquitectura del Sistema](#arquitectura-del-sistema)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Estructura del Proyecto](#estructura-del-proyecto)

<br>

## Características

- **Simulación de Vehículos:** Los vehículos se crean con una dirección (Norte o Sur) y velocidad aleatoria.
- **Entrada controlada al puente:** Se utiliza una cola de prioridad para que los vehículos crucen por orden de dirección.
- **Gestión de concurrencia:** Uso de semáforo para evitar condiciones de carrera y colisiones virtuales.
- **Visualización en tiempo real:** Se muestra el estado de cada vehículo, incluyendo paradas y desconexiones inesperadas.
- **Gestión de desconexiones:** Si un vehículo se cae dentro del puente, se libera automáticamente para no bloquear la simulación.
- **Cliente y Servidor independientes:** Separación clara entre lógica de presentación (cliente) y lógica de control (servidor).

<br>

## Arquitectura del Sistema

El sistema se divide en **cliente** y **servidor**, ambos comunicándose mediante `NetworkStream`. El servidor contiene la lógica principal (carretera, puente y sincronización), mientras que el cliente representa un vehículo individual.

1. **Vehículo:** Cada vehículo tiene ID, posición, velocidad, dirección, estado (`Parado`, `Acabado`, `Desconectado`) y se mueve en intervalos.
2. **Servidor:** Controla el paso por el puente mediante un semáforo y una cola prioritaria. Además, coordina y actualiza el estado global de la carretera.
3. **Cliente:** Mueve su vehículo localmente y se sincroniza con el servidor. Muestra por consola su estado y la carretera.
4. **Infraestructura:** Contiene utilidades de red (`NetworkStreamClass`) y consola (`Consola.cs`) para estandarizar la comunicación.

<br>

## Tecnologías Utilizadas

- **Lenguaje:** C#
- **Comunicación:** `TcpClient`, `NetworkStream`, comunicación cliente-servidor TCP.
- **Hilos y Concurrencia:** `Task`, `async/await`, `SemaphoreSlim`, `PriorityQueue`, `lock`.
- **Estructuras de Datos Concurrentes:** `ConcurrentDictionary`, `List`.
- **Serialización:** `XmlSerializer` para enviar objetos completos por red.
- **Sincronización visual:** Bloqueos con `lock` para evitar condiciones de carrera al pintar en consola.

<br>

## Estructura del Proyecto

<table align="center" border="6px">
    <tr>
        <td>
            <pre>
                Ejercicio3
                │
                ├── 🧑‍💻 Cliente
                │   ├── 📁 Handlers
                │   │   ├── CarreteraHandler.cs
                │   │   ├── VehiculoHandler.cs
                │   │   └── ClienteHandshake.cs
                │   └── 📄 Program.cs
                │
                ├── 🧱 Dominio
                │   ├── 🚗 Carretera
                │   │   └── Carretera.cs
                │   ├── 🌉 Puente
                │   │   └── Puente.cs
                │   └── 🚦 Vehiculo
                │       └── Vehiculo.cs
                │
                ├── 🧰 Infraestructura
                │   ├── 🌐 NetworkStreamClass
                │   │   └── NetworkStreamClass.cs
                │   └── 🛠️ Utils
                │       └── Consola.cs
                │
                ├── 🖥️ Servidor
                │   ├── 📁 Handlers
                │   │   ├── HandshakeHandler.cs
                │   │   └── RecepcionVehiculoHandler.cs
                │   ├── 📁 Modelos
                │   │   └── Cliente.cs
                │   ├── ClienteManager.cs
                │   └── Program.cs
            </pre>
        </td>
    </tr>
</table>

<div align="center">

###### © Sammy

</div>
