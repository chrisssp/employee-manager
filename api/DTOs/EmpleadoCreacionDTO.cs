using System.ComponentModel.DataAnnotations;

namespace EmpresaApi.DTOs
{
    public class EmpleadoCreacionDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [MaxLength(50)]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [MaxLength(100)]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe asignar al menos un rol")]
        [MinLength(1, ErrorMessage = "Debe proporcionar al menos el ID de un rol")]
        public List<int> RolesIds { get; set; } = new();
    }
}
