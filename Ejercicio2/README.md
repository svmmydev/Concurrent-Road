<div align="center">

# EJERCICIO 2 - Intercambio de conexión entre vehículos

</div>

<br>

### Descripción

Para esta tarea, se buscan los siguientes objetivos:

- [Etapa 8](#etapa-8) - Conexión servidor/cliente.
- [Etapa 9](#etapa-9) - Aceptación de clientes.
- [Etapa 10](#etapa-10) - Asignar un ID único a cada vehículo (cliente).
- [Etapa 11](#etapa-11) - Obtener el NetworkStream.
- [Etapa 12](#etapa-12) - Programar métodos EscribirMensajeNetworkStream y LeerMensajeNetworkStream.

<br>

### Etapa 8
#### *Métodos clase NetworkStreamClass*

> Se implementa todo el código necesario aprendido para la lectura y escritura de los datos mediante un bucle el cual permite iterar hasta vaciar el NetworkStream.

<br>

> [!WARNING]
> Tuve que implementar un header que estableciera la longitud del mensaje que se va recibir. Para un modelo sin `async`, es arriesgado pero no obligatorio, sin embargo, para un modelo `TAP` es obligatorio usar headers, de lo contrario podría estar leyendo menos bytes de los necesarios. O de lo contrario, leer
> más de un mensaje a la vez y mezclar contenido.

<br>

### Etapa 9
#### *Crear y enviar datos de un vehículo*

> Para esta tarea, como se realizó el el README posteriormente, decio publicar la siguiente prueba donde se puede pareciar como 3 vehículos están en asimetría total cada uno con su propio camino y velocidad.

<br>

<div align="center">

![Imagen de la prueba](../Assets/Images/8-lista-vehiculos.png)

</div>

<br>

### Etapa 10
#### *Asignación ID*

> **APM** / **TAP**:
> 
> Para ambos modelos se ha implementado de la misma manera. Se ha utilizado la clase Interlock, la cual permite acceder a la variable `idUnico` de forma segura evitando condición de carrera. Se incrementa en 1 por cada cliente generado. De una forma sencilla podemos mantenter un sistema de generación de IDs
> únicos.
>
> `int clienteId = Interlocked.Increment(ref IdUnico);`

<br>

<div align="center">

![Imagen de la prueba](../Assets/Images/5-id-control.png)

</div>

<br>

## Etapa 11
#### *Captura del NetworkStream*

> **APM** / **TAP**:
> 
> De la misma manera que para el ID, esto no es dificil de lograr en los dos modelos, es lo mismo. Se consigue el NetworkStream de cada cliante justo en el momento que la conexión se ha establecido. La sentencia es bastante sencilla:
>
> `NetworkStream netwS = cliente.GetStream();`

<br>

### Etapa 12
#### *Métodos lectura y escritura*

> **APM**:
> 
> Mediante el modelo APM, se inicia la lectura preparando un buffer de lectura. Seguidamente se llama al correspondiente método mediante el BeginRead pasándole el método callback por parámetro. Dentro de este "método delegado" se ejecuta el código de forma asíncrona, permitiendo así la lectura en segundo
> plano. Con el uso del EndRead, el sistema termina de leer, y con un sencillo control de condiciones, se puede llamar de forma recursiva al método BeginRead para que en todo momento, se puede mantener una escucha de datos constante. Para la escritura es más de lo mismo, se convierte el mensaje a bytes y se
> usa el buffer junto al delegado (callback) para ejecutar la escritura de forma asíncrona, validando su finalización con el uso del EndWrite.
> 
> **TAP**:
> 
> Con este modelo, la cosa se vuelve bastante más sencilla. Rescata el método de lectura y escritura más antiguo pero con la implementación del async/await. Nada de segundos métodos delegados ni callbacks explícitos, se prepara un MemoryStream, un buffer, y mediante el uso de un bucle do/while se leen los datos
> de forma continua hasta que `netwS.DataAvailable()` determina que ya no quedan más datos que leer. Lo mismo para la escritura pero con el uso del `WriteAsync()`.

<br>

### Etapa 6
#### *Handshake*

> **APM**:
> 
> El Handshake con este modelo se vuelve un poco más tedioso. La forma má correcta de realizarlo es mediante la implementación de callbacks anidados uno dentro del otro leyendo y escribiendo, llamando y buscando respuesta. << Te mando INICIO, ¡has recibido INICIO? (siguiente paso), te he asignado un ID, me
> confirmas que has recibido el mismo ID?, todo ok. Todos los pasos van anidados (un poco feo de mantener para mi gusto).
> 
> **TAP**:
> 
> Otro gallo canta. Nada de callbacks anidados, condiciones sencillas y pasos marcados con los correspondientes awaits y sus llamadas a métodos de lectura y escritura. Mismos pasos, ¿INICIO? ¿Mismo ID confirmado? todo ok.

<br>

### Etapa 7
#### *Almacenaje de información clientes*

> **Respuesta según el código**:
> 
> El primer paciente que entra en consulta es el Paciente 1. Si partimos de la base que genera el enunciado, cada llegada de paciente es separada por un lapso de tiempo de 2 segundos. En mi caso, siempre entrará primero el Paciente 1. Si, en lugar de mi código actual, hubiese creado una lista de 4 hilos y hubiese decidido que la ejecución de los mismos se hiciese de forma random, sacando aleatoriamente hilos de la lista, el orden de entrada podría ser diferente en cada ejecución.
> 
> Pero si leemos detenidamente el enunciado, dice: << Llega un paciente cada 2 segundos >>. En mi opinión, en esa sentencia, se confirma que la creación es en con ese intervalo, por lo tanto, siempre se ejecutará el mismo hilo en primer lugar.
> 
> En el segundo caso que he expuesto estaríamos dando por hecho que ya existen esos 4 pacientes (la lista de hilos pre-creada), y creo que no es el objetivo de esta tarea. Para que en este caso fuese totalmente válido en relación al enunciado, debería decir algo como: << En una sala de espera, donde hay 4 pacientes esperando, se les llama para entrar en consulta cada 2 segundos >>.

<br>

<div align="center">

![Imagen de la prueba](../Assets/Images/6-lista-vehiculos.png)

</div>

<br>
