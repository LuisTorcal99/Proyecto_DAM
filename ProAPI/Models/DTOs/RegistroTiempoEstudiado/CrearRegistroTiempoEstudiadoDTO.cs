namespace RestAPI.Models.DTOs.RegistroTiempoEstudiado
{
    public class CrearRegistroTiempoEstudiadoDTO
    {
        public int UsuarioId { get; set; } 
        public DateTime Fecha { get; set; }  
        public int AsignaturaID { get; set; }  
        public TimeSpan TiempoEstudiado { get; set; }
    }
}
