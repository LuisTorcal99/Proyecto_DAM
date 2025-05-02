namespace RestAPI.Models.Entity
{
    public class RegistroTiempoEstudiadoEntity
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public int AsignaturaID { get; set; }
        public TimeSpan TiempoEstudiado { get; set; }
    }
}
