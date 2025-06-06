<div align="center">

# EJERCICIO 2 - Intercambio de conexión entre vehículos

</div>

<br>

### Descripción

Para esta tarea, se buscan los siguientes objetivos:

- [Etapa 8](#etapa-8) - Programar métodos de la clase NetworkStreamClass.
- [Etapa 9](#etapa-9) - Crear y enviar datos de un vehículo.
- [Etapa 10](#etapa-10) - Mover vehículos.
- [Etapa 11](#etapa-11) - Enviar datos del Servidor a todos los clientes.
- [Etapa 12](#etapa-12) - Recepción de la información del Servidor en el Cliente.

<br>

### Etapa 8
#### *Métodos clase NetworkStreamClass*

> Se implementa todo el código necesario aprendido para la lectura y escritura de los datos mediante un bucle el cual permite
> iterar hasta vaciar el NetworkStream.

<br>

> [!IMPORTANT]
> Tuve que implementar un header que estableciera la longitud del mensaje que se va recibir. Para un modelo sin `async`, es
> arriesgado pero no obligatorio, sin embargo, para un modelo `TAP` es obligatorio usar headers, de lo contrario podría estar
> leyendo menos bytes de los necesarios. O de lo contrario, leer más de un mensaje a la vez y mezclar contenido.

<br>

### Etapa 9
#### *Crear y enviar datos de un vehículo*

> En esta tarea, primero se almacena el cliente que ha superado el `Handshake`, depués se ejecuta una tarea `Task` para gestionar
> un nuevo vehículo. Dentro del método que gestiona el nuevo vehículo, se instancia y se procede a leer los datos del vehículo
> cliente mediante un bucle `while(true)`, para posteriormente mostrarlos por pantalla.

<br>

<div align="center">

![Imagen de la prueba](../Assets/Images/7-mostrar-vehiculos.png)

</div>

<br>

### Etapa 10
#### *Mover los vehiculos*

> Para cumplir el objetivo de esta, se implementa un bucle for iterativo de 0 a 100, y mediante un `Thread.Sleep("cuanto mayor
> número más lento va el vehículo")` se consigue simular el movimiento de dicho vehículo. A cada iteración se envía la nueva
> información al Servidor, y dentro de este, en el mismo método que la etapa anterior, dentro de un bucle `while` iteramos
> hasta que el vehiculo haya acabado. Finalmente, si el vehículo ha terminado el recorrido, también se mostrará la desconexión
> del mismo.

<br>

<div align="center">

![Imagen de la prueba](../Assets/Images/8-mover-vehiculos.png)

</div>

<br>

> [!TIP]
> Como bien se ha dicho, la práctica dio un giro de 90º para implementar el modelo `TAP`, y una de las cosas que se me olvidó
> cambiar fue el `Thread.Sleep()` por el `Task.Delay()`. Si tenemos un modelo TAP y hacemos uso del Thread.Sleep(), estamos
> obligando al sistema a utilizar más recursos de los que debería incluso podría afectar al correcto funcionamiento del
> programa bloqueando el hilo por completo. Esto se corrige en la última parte de la práctica `Ejercicio 3` haciendo uso del
> Task.Delay().

<br>

### Etapa 11
#### *Enviar datos del servidor a todos los clientes*

> El Servidor recibe los datos del vehículo mediante la lectura que mencionamos anteriormente, los almacena en una nueva
> instancia `Vehiculo` y lo añade a la lista de vehículos que hay en la carretera, tal y como hemos visto en la anterior etapa.
> Posteriormente se envía el estado de la carretera con los cambios mediante un método específico. Este método, itera sobre todos
> los clientes que han sido registrados en el Servidor y les envía el estado de la carretera mediante el método de escritura que
> también se mencionó etapas atrás. Si ocurre algún error, elimina al cliente de la lista.

<br>

### Etapa 12
#### *Recepción de la información del Servidor en el cliente*

> Desde el Main de Cliente, se ejecuta una tarea `Task` que actualiza la carretera en segundo plano. El método que se emplea es
> un método que contiene un `while(true)` el cual está constantemente leyendo el Stream, permaneciendo a la escucha de nuevos
> cambios en la carretera. Una vez los tiene y los actualiza, ejecuta el siguiente método que permite mostrar la carretera. En
> él, se itera sobre todos los vehículos que hay en carretera y mediante un limpiado constante de la consola, va mostrando el
> recorrido que hace cada vehículo con sus respectivos datos.

<br>

<div align="center">

![Imagen de la prueba](../Assets/Images/9-mostrar-carretera-en-cliente.png)

</div>

<br>
