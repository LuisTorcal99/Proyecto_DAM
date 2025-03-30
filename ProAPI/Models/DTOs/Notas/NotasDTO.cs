namespace RestAPI.Models.DTOs.Notas
{
    public class NotasDTO : CreateNotasDTO
    {
        public int Id { get; set; }
        public int IdAsignatura { get; set; }
        public int IdEvento { get; set; }
    }
}
