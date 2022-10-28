# PEC 1 - Carreras Contrarreloj

## Cómo jugar
Al iniciar el juego, se escoge un coche, de entre dos, y un circuito, para lo que también vemos dos opciones.
Cuando se selecciona el circuito, se inicia la escena de juego escogida.
Una vez en esta, la carrera puede comenzar. Lo hará en cuanto el jugador pase por la línea de salida por primera vez.
Para controlar el coche, se tendrán que usar las teclas W, para acelerar, A y D para girar y S para frenar o ir marcha atrás.
Cuando se inicie una vuelta, el contador empezará a correr.
Al finalizar una vuelta, se guardará y se mostrará el tiempo de esta, además de actualizar el tiempo de la mejor vuelta de la carrera.
Además, al finalizar la primera vuelta, aparecerá el coche fantasma, que mostrará el recorrido de la mejor vuelta.
Cuando se acabe la carrera, después de tres vueltas, se podrá ver una repetición de la carrera completa.

## Estructura e Implementación
El juego se divide en tres escenas, el menú y los dos circuitos.
Para cada uno de los circuitos, se ha creado un terreno sobre los que se ha añadido una carretera que servirá de circuito. Esto también se ha hecho en el menú para usarlo de fondo.

Como hemos comentado, desde el menú se podrá escoger entre dos coches y entre dos terrenos. La elección de terreno simplemente indica que escena se cargará. Para la elección del coche, se ha creado un ScriptableObject, ChosenCarData, que guarda los prefabs tanto del coche como del fantasma asociado a este. Al pulsar el botón, este dato se guardará en un GameManager que implementa el patrón Singleton con la intención de que sea accesible desde el resto de clases y que no se destruya al cambiar de escena. Esto mismo hará el MenuManager, que contiene los métodos de manejo de escenas, y será necesario para regresar al menú o reiniciar la carrera. El uso del patrón Singleton conlleva la necesidad de una clase intermedia que permita que los botones de las escenas accedan a los métodos de los managers al ser pulsados, puesto que se pierde la referencia que se le pueda haber añadido a su OnClick. A esta clase la hemos llamado UIButtons, y contiene métodos de conexión con los dos managers, usando la instancia estática del singleton.

Una vez se han seleccionado el coche y el circuito, se cargará la escena correspondiente.
Las escenas de circuito contienen varios elementos destacables: un LevelManager, un GhostManager, una LapLine, un StartPoint y un RepeatCameraManager.

El StartPoint definirá el punto donde se debe instanciar el coche seleccionado para el inicio de la carrera.
Esto lo hará el LevelManager, que, al iniciar la escena, recuperará el coche y el fantasma del GameManager y los instanciará en la posición indicada.
Este manager, además, implementa una versión reducida del patrón Singleton, de modo que sea único y accesible solo a nivel de la escena. Gracias a esto, el LevelManager nos servirá de conexión entre los diferentes elementos de la escena. Este se encargará de dar acceso a la instancia tanto del coche como del ghost en la escena, de mostrar por pantalla UI de fin de carrera y de activar y desactivar las cámaras de repetición de carrera.

Por su parte, la LapLine se encarga de detectar cuando se empieza y/o acaba una vuelta y de informar al resto de elementos de ello, a través de eventos Action. Comprueba también si la vuelta ha sido mejor que las anteriores, en cuyo caso avisa para que se guarde el nuevo fantasma.

Esta acción de guardar al fantasma de la mejor vuelta, es una ampliación del GhostManager de los apuntes de la asignatura. Al iniciar cada vuelta se reinician los valores guardados en lo que llamamos currentLapSO, instancia del ScriptableObject LapData que guarda la última vuelta, y se empiezan a grabar los datos de posición y rotación del coche. Al finalizar la vuelta, si es mejor que las anteriores o es la primera, se recorrerán los datos guardados de la vuelta y se almacenarán en bestLapSO, que contendrá los datos de la mejor vuelta. Además, a partir de la segunda vuelta, los datos que se guarden en este, serán reproducidos por el fantasma, mostrando así al jugador como va con respecto al mejor tiempo.
Además, durante toda la carrera, el GhostManager irá almacenando los datos de la misma, que podrán ser reproducidos por el mismo coche una vez acabada la carrera.

Esta reproducción se podrá ver con la cámara de juego o con un conjunto de CinemachineVirtualCameras que simulan una reproducción por televisión de la carrera. De este control se encarga el RepeatCameraManager. Hemos colocado cuatro virtual cameras alrededor del coche para esta simulación a las que cambiamos la prioridad en un intervalo aleatorio de tiempo. Cuando se activa la repetición, se escoge un valor aleatorio de la lista de cámaras para iniciar la reproducción y se aleatoriza el tiempo que debe pasar antes del siguiente cambio de cámara. Además, se usa una segunda lista para decidir qué cámaras pueden ser usadas en el siguiente cambio, excluyendo así la cámara activa.
Para controlar el número de cámaras y guardarlas en las dos listas, se ha creado una clase VCamera. que al iniciar la escena, notifica al RepeatCameraManager, mediante una Action, de su existencia. Además, esta clase nos sirve para recuperar el coche del LevelManager y asignarlo como Follow y LookAt de cada una de las Virtual Cameras.

Para el control de la velocidad del jugador cuando se sale de la carretera, se ha creado un trigger por rueda que se encarga de detectar cuando se sale de la carretera, en cuyo caso llamada al CarController para reducir el Full Torque del coche, o cuando vuelve a entrar a ella, en cuyo caso resetea ese valor.

Finalmente, para el control de la UI y para mostrar los tiempos de cada vuelta, de la mejor vuelta y de la vuelta actual, se crea una clase UILapTimes y un enum TypeOfLapText que definirán qué Text se actualiza. De este modo, desde LapLine, y a través de Actions, se invocará al método encargado de actualizar el texto, que comprobará, en base al tipo de Lap Text si debe reaccionar o no.

## Problemas conocidos
No se ha implementado una solución para evitar que el jugador haga trampa en la línea de salida y no recorra el circuito completo.

Los terrenos no son optivos y pueden provocar que el jugador se quede atascado y no pueda mover el coche. Para esto, tenemos la opción de reiniciar la carrera pero puede llegar a ser muy molesto.

El jugador puede atravesar el agua sin problema.

La reproducción de la mejor vuelta y de la carrera completa se ven con un efecto de "jitter" cuyo origen desconozco. Este es especialmente notable con la cámara de juego.

## Vídeo
