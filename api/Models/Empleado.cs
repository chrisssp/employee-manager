using System.ComponentModel.DataAnnotations;

namespace EmpresaApi.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ApellidoMaterno { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBaja { get; set; }

        public List<Rol> Roles { get; set; } = new();
    }
}
