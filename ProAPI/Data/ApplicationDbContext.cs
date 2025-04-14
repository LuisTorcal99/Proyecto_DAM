using System.Data;
using System.Diagnostics.Metrics;
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

            // Relación entre Asignatura y Notas (uno a muchos)
            modelBuilder.Entity<AsignaturaEntity>()
                .HasMany(a => a.Notas)
                .WithOne(n => n.Asignatura)
                .HasForeignKey(n => n.IdAsignatura)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Asignatura y Eventos (uno a muchos)
            modelBuilder.Entity<AsignaturaEntity>()
                .HasMany(a => a.Eventos)
                .WithOne(e => e.Asignatura)
                .HasForeignKey(e => e.IdAsignatura)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Evento y Nota (uno a uno)
            modelBuilder.Entity<EventoEntity>()
                .HasOne(e => e.Nota)
                .WithOne(n => n.Evento)
                .HasForeignKey<NotaEntity>(n => n.IdEvento)
                .OnDelete(DeleteBehavior.Restrict); 

            // Relación entre Usuario y Asignatura (uno a muchos)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Asignaturas)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict); 

            // Relación entre Usuario y Evento (uno a muchos)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Eventos)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Usuario y Nota (uno a muchos)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notas)
                .WithOne(n => n.Usuario)
                .HasForeignKey(n => n.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Usuario y AppUser (uno a uno)
            modelBuilder.Entity<User>()
              .HasOne(u => u.AspNetUser)
              .WithOne()
              .HasForeignKey<User>(u => u.AspNetUserId)
              .OnDelete(DeleteBehavior.Restrict);

            // ALTER TABLE ProyectoDAM.dbo.AspNetUsers DROP CONSTRAINT FK_AspNetUsers_Users_UserId;
            // DROP TABLE ProyectoDAM.dbo.AspNetUsers;
            // DROP TABLE ProyectoDAM.dbo.Users;
        }

        // DbSets para cada entidad
        public DbSet<User> Users { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<NotaEntity> Notas { get; set; }
        public DbSet<EventoEntity> Eventos { get; set; }
        public DbSet<AsignaturaEntity> Asignaturas { get; set; }
    }
}
