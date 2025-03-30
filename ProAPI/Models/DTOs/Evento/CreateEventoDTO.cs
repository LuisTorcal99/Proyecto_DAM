namespace RestAPI.Models.DTOs.Evento
{
    public class CreateEventoDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Porcentaje { get; set; }
        public DateTime Fecha { get; set; }
    }
}
