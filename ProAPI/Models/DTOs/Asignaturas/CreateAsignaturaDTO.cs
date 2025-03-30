namespace RestAPI.Models.DTOs.Asignaturas
{
    public class CreateAsignaturaDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Creditos { get; set; }
    }
}
