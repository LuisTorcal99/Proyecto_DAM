namespace RestAPI.Models.Entity
{
    public class GamificacionEntity
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; } 
        public DateTime Fecha { get; set; } 
        public string TipoDeLogro { get; set; }
        public int Puntos { get; set; } 
        public string Descripcion { get; set; }  
    }
}
