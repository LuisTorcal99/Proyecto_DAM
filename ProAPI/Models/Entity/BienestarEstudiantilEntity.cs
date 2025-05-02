namespace RestAPI.Models.Entity
{
    public class BienestarEstudiantilEntity
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public string EstadoDeAnimo { get; set; }
        public int NivelDeEstres { get; set; }
        public string Sugerencias { get; set; }
    }
}
