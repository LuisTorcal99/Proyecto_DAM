# Proyecto Final de DAM - Luis Ángel Torcal Carrillo

## Descripción

El proyecto es una aplicación de escritorio diseñada para estudiantes, permitiéndoles gestionar sus tareas, exámenes y otros eventos académicos. Las principales funcionalidades son:

- **Gestión de tareas y exámenes**: Los estudiantes pueden agregar, editar y eliminar tareas o exámenes, especificando fechas de entrega y descripciones.
- **Registro de notas**: Los estudiantes pueden registrar las notas obtenidas en cada actividad y el porcentaje de impacto en la evaluación final.
- **Cálculo automático de calificación**: El sistema calcula la calificación final en función de las notas obtenidas y los porcentajes asignados a cada tarea.
- **Etiquetas personalizables**: Los estudiantes pueden etiquetar sus actividades como "Examen", "Tarea" o "Nota" para una mejor organización.
- **Seguimiento del progreso**: Cada actividad tiene un estado de progreso que puede ser "Pendiente", "EnProceso" o "Completado", permitiendo a los estudiantes realizar un seguimiento del avance de sus tareas y exámenes.
- **Notificaciones con RabbitMQ**: El sistema envía notificaciones por gmail sobre eventos próximos.
- **Carga de cursos y asignaturas**: Existen presets para cargar asignaturas y cursos de forma rápida.
- **Gestión de faltas de asistencia**: Los estudiantes pueden registrar las faltas de asistencia, y la aplicación calcula cuántos días faltan para llegar al límite de faltas permitidas.
- **Modo claro y oscuro**: Los usuarios pueden cambiar entre el modo claro y oscuro.
- **Exportación de datos**: Los estudiantes pueden exportar sus datos a Excel y PowerPoint.
- **Autenticación y Autorización**: Cada usuario tiene su propio perfil y sus datos son privados.
- **Historial de Actividades**: Visualiza el historial de tareas entregadas, exámenes realizados y las calificaciones obtenidas.
- **Actividades**: Visualiza las actividades de las proximas tareas y exámenes y la fecha en la que se realiza, cuenta con colores para hacer mas visual los eventos que estan por suceder.
- **Temporizador Pomodoro**: Organiza sesiones de estudio usando el método Pomodoro con intervalos de trabajo y descanso.
- **Filtrar eventos**: Se pueden filtrar los eventos por **tipo** (tarea, examen, nota), **progreso** (Pendiente, EnProceso, Completado) y **asignatura**.
- **Sistema de logros: Los estudiantes desbloquean logros al completar tareas, estudiar durante cierto tiempo o alcanzar metas.**
- **Ranking de logros: Se muestra un ranking con los usuarios que han conseguido más logros en la aplicación.**
- **Pestaña de bienestar estudiantil: Los estudiantes pueden registrar su estado de ánimo y nivel de estrés diario, con un gráfico que visualiza la evolución a lo largo del tiempo.**
- **Perfil del usuario: Pestaña donde cada estudiante puede ver estadísticas y gráficos personalizados sobre su progreso y rendimiento académico, así como editar su información personal.**

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
   
3. **ApiRest**:
    - Al lanzar la API (dotnet run), se crean automáticamente la base de datos, el login, las migraciones y los permisos necesarios.
    - No es necesario ejecutar manualmente ningún script SQL ni comandos de migración.
     
4. **Ejecutar la aplicación**:
    - Abre la carpeta raíz del proyecto.
    - Ejecuta el archivo lanzador.bat o lanzadorConsola.bat (dependiendo de si deseas ver la consola o no).
    - El script iniciará automáticamente tanto la API como la aplicación WPF.

5. **Acceso a la aplicación**:
    Al iniciar la aplicación, los estudiantes pueden registrarse, iniciar sesión y comenzar a gestionar sus tareas, exámenes y otras actividades académicas.