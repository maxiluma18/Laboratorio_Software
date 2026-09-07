# OBBY RACE - Prototipo 3D Multijugador
Este repositorio contiene el código fuente y los assets de un prototipo de videojuego de plataformas tridimensional, desarrollado como proyecto universitario. Inspirado en mecánicas de títulos como Fall Guys, el juego propone carreras de obstáculos dinámicas con un fuerte enfoque en la experimentación de físicas, sistemas de colisiones y sincronización en red.

El proyecto fue planificado y ejecutado en 4 Sprints mediante la metodología ágil Scrum, gestionado a través de Jira por un equipo de 4 desarrolladores.

## 🎮 Modos de Juego
* Un Jugador (Singleplayer): Carrera de supervivencia contra los obstáculos. Incluye un selector de dificultad que determina la cantidad de vidas disponibles.
* Multijugador Local (Camara dividida: Dos usuarios compiten simultáneamente en la misma terminal compartiendo pantalla.
* Multijugador Online (Internet): Soporte para hasta 4 jugadores conectados simultáneamente mediante creación de salas (Lobby).

## ✨ Características Principales
* Mecánicas de Movimiento y Físicas: Control en tercera persona con simulación de gravedad, saltos y colisiones con superficies y plataformas.
* Diseño de Niveles Lineal (Obby): Recorrido con obstáculos estáticos y dinámicos interactivos (ej. martillos rotativos).
* Sistema de Penalización y Checkpoints: Al caer al vacío o chocar con trampas, el jugador reaparece en el último punto de control alcanzado.
* Sincronización en Red (Client-Authoritative): Arquitectura optimizada con Unity Netcode que prioriza la respuesta instantánea del cliente, sincronizando posición, rotación, saltos y caídas en tiempo real.
* Condiciones de Victoria/Derrota: El primer jugador en llegar a la meta se consagra ganador, ejecutando el estado de derrota para el resto. En modo solitario, la derrota ocurre al agotar las vidas.
* Cámara Dinámica: Sistema de acercamiento y alejamiento (zoom) mediante la rueda del ratón, disponible en modo Solitario y Multijugador Online.
* Diferenciación Visual: Asignación automática de skins o variaciones visuales según el jugador para facilitar la identificación.
* Experiencia Audiovisual y UI: Música ambiental, efectos de sonido (SFX) por acciones, control global de muteo, panel de ayuda in-game y mecanismos de desconexión segura al menú.

## 💻 Tecnologías y Herramientas
* Motor Gráfico: Unity 6000.5.9f1
* Networking: Unity Netcode for GameObjects (NGO) & Unity Relay (Services)
* Lenguaje: C#
* Gestión de Proyecto: Jira, Scrum
* Control de Versiones: Git / GitHub

## ⌨️ Controles Básicos
* Movimiento: Teclas W A S D o Flechas Direccionales
* Salto: Barra Espaciadora o Control
* Zoom de Cámara: Rueda del ratón
* Pausa / Menú: ESC

## 🚀 Instalación y Ejecución
* Para abrir y modificar el proyecto en un entorno de desarrollo:
* Clonar este repositorio: git clone [https://github.com/maxiluma18/Laboratorio_Software](https://github.com/maxiluma18/Laboratorio_Software)
* Asegurarse de tener instalada la versión exacta del editor: Unity 6000.5.9f1.
* Abrir el proyecto desde Unity Hub.
* Cargar la escena inicial: Assets/Scenes/MainMenu.unity.

*(Nota técnica sobre la red: La implementación multijugador utiliza Unity Relay configurado de forma abierta sin requerir una cuenta de Unity Services estrictamente vinculada por parte del usuario final para poder realizar pruebas de conectividad).*

## 👥 Equipo de Desarrollo
* Maximiliano Martinez - Scrum Master
* Alejandro Markic - Desarrollador - [Perfil de Alejandro](https://github.com/Ale-Markic)
* Pablo Galván - Desarrollador - [Perfil de Pablo](https://github.com/PabloAgustinGalvan)
* Brandon Apaza - Tester- [Perfil de Brandon](https://github.com/Brandon16082001)
