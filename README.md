# Proyecto Final de DAM - Luis Ángel Torcal Carrillo

## Descripción

El proyecto es una aplicación de escritorio diseñada para estudiantes, permitiéndoles gestionar sus tareas, exámenes y otros eventos académicos. Las principales funcionalidades son:

- **Gestión de tareas y exámenes**: Los estudiantes pueden agregar, editar y eliminar tareas o exámenes, especificando fechas de entrega y descripciones.
- **Registro de notas**: Los estudiantes pueden registrar las notas obtenidas en cada actividad y el porcentaje de impacto en la evaluación final.
- **Cálculo automático de calificación**: El sistema calcula la calificación final en función de las notas obtenidas y los porcentajes asignados a cada tarea.
- **Etiquetas personalizables**: Los estudiantes pueden etiquetar sus actividades como "Examen", "Tarea" o "Nota" para una mejor organización.
- **Seguimiento del progreso**: Cada actividad tiene un estado de progreso que puede ser "Pendiente", "EnProceso" o "Completado", permitiendo a los estudiantes realizar un seguimiento del avance de sus tareas y exámenes.
- **Notificaciones con RabbitMQ**: El sistema envía notificaciones sobre eventos próximos.
- **Carga de cursos y asignaturas**: Existen presets para cargar asignaturas y cursos de forma rápida.
- **Gestión de faltas de asistencia**: Los estudiantes pueden registrar las faltas de asistencia, y la aplicación calcula cuántos días faltan para llegar al límite de faltas permitidas.
- **Modo claro y oscuro**: Los usuarios pueden cambiar entre el modo claro y oscuro.
- **Exportación de datos**: Los estudiantes pueden exportar sus datos a Excel y PowerPoint.
- **Autenticación y Autorización**: Cada usuario tiene su propio perfil y sus datos son privados.
- **Historial de Actividades**: Visualiza el historial de tareas entregadas, exámenes realizados y las calificaciones obtenidas.
- **Actividades**: Visualiza las actividades de las proximas tareas y exámenes y la fecha en la que se realiza, cuenta con colores para hacer mas visual los eventos que estan por suceder.
- **Temporizador Pomodoro**: Organiza sesiones de estudio usando el método Pomodoro con intervalos de trabajo y descanso.
- **Filtrar eventos**: Se pueden filtrar los eventos por **tipo** (tarea, examen, nota), **progreso** (Pendiente, EnProceso, Completado) y **asignatura**.

## Tecnologías Utilizadas

- C#
- WPF (Windows Presentation Foundation)
- SQL Server
- RabbitMQ
- API Rest
- Docker

## Instrucciones de Uso

1. **Clonar el repositorio**:
    ```bash
    git clone https://github.com/LuisTorcal99/Proyecto_DAM.git
    ```
    
2. **Configurar Docker**:
    Asegúrate de tener Docker Desktop instalado y configurado en tu máquina. Los contenedores de Docker se crearán, levantarán y cerrarán automáticamente.

3. **Base de Datos**:
    El archivo `BBDD.sql` contiene los scripts necesarios para crear la base de datos y los usuarios. Si estás utilizando SQL Server localmente, ejecuta este script para crear la base de datos y conceder los permisos necesarios al usuario.
   
5. **ApiRest**:
   - Crear la migración con Entity Framework en la consola de desarrollador de paquetes (add-migration InitialCreate, update-database).
   - **Lanzamiento de la API** con `dotnet run`.

6. **Ejecutar la aplicación**:
    - Abre la solución en Visual Studio o Visual Studio Code.
    - Compila y ejecuta la aplicación.

7. **Acceso a la aplicación**:
    Al iniciar la aplicación, los estudiantes pueden registrarse, iniciar sesión y comenzar a gestionar sus tareas, exámenes y otras actividades académicas.
   
