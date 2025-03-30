using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Entity;

namespace RestAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación entre User y AppUser (Identity)
            modelBuilder.Entity<User>()
                .HasOne(u => u.AspNetUser)
                .WithOne()
                .HasForeignKey<User>(u => u.AspNetUserId);

            // Relación de la Nota con la Asignatura
            modelBuilder.Entity<NotaEntity>()
                .HasOne<AsignaturaEntity>()
                .WithMany(a => a.Notas) // Asignaturas tiene muchas notas
                .HasForeignKey(n => n.IdAsignatura)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación de la Nota con el Evento
            modelBuilder.Entity<NotaEntity>()
                .HasOne<EventoEntity>()
                .WithMany(e => e.Notas) // Eventos tiene muchas notas
                .HasForeignKey(n => n.IdEvento)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación de la Nota con el Usuario
            modelBuilder.Entity<NotaEntity>()
                .HasOne<User>()
                .WithMany(u => u.Notas) // Un usuario tiene muchas notas
                .HasForeignKey(n => n.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación de la Asignatura con el Usuario
            modelBuilder.Entity<AsignaturaEntity>()
                .HasOne<User>()
                .WithMany(u => u.Asignaturas) // Un usuario tiene muchas asignaturas
                .HasForeignKey(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación de Evento con el Usuario
            modelBuilder.Entity<EventoEntity>()
                .HasOne<User>()
                .WithMany(u => u.Eventos) // Un usuario tiene muchos eventos
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar el tipo de dato `Porcentaje` en Evento (precision)
            modelBuilder.Entity<EventoEntity>()
                .Property(e => e.Porcentaje)
                .HasPrecision(5, 2); // 5 dígitos en total, 2 decimales
        }

        // DbSets para cada entidad
        public DbSet<User> Users { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<NotaEntity> Notas { get; set; }
        public DbSet<EventoEntity> Eventos { get; set; }
        public DbSet<AsignaturaEntity> Asignaturas { get; set; }
    }
}
