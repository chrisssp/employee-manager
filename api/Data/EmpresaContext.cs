using EmpresaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpresaApi.Data
{
    public class EmpresaContext : DbContext
    {
        public EmpresaContext(DbContextOptions<EmpresaContext> options) : base(options) { }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<LogTransaccion> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasIndex(e => e.Correo)
                    .IsUnique()
                    .HasDatabaseName("IX_Empleado_Correo_Unico");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(50);

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Activo)
                    .HasDefaultValue(true);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.Property(r => r.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(r => r.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<LogTransaccion>(entity =>
            {
                entity.Property(l => l.VerboHttp)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(l => l.Endpoint)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(l => l.Payload)
                    .HasMaxLength(4000);

                entity.Property(l => l.Fecha)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
        }

        // Interceptar cambios para actualizar fechas automáticamente
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Empleado>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified && entry.Entity.Activo == true)
                {
                    entry.Entity.FechaActualizacion = System.DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    var activoOriginal = entry.OriginalValues.GetValue<bool>(nameof(Empleado.Activo));
                    var activoActual = entry.Entity.Activo;

                    // Si cambió de true a false, es un soft delete
                    if (activoOriginal == true && activoActual == false)
                    {
                        entry.Entity.FechaBaja = System.DateTime.UtcNow;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
