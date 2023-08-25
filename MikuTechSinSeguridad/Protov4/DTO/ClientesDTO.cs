using System.ComponentModel.DataAnnotations;

namespace Protov4.DTO
{
    public class ClientesDTO
    {
        [Key]
        public int? id_cliente { get; set; }

        [EmailAddress]
        [StringLength(25)]
        [Required(ErrorMessage = "Campo requerido")]
        public string correo_nuevo { get; set; }

        [MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [MinLength(3, ErrorMessage = "Mínimo 3 caracteres")]
        [StringLength(15)]
        [Required(ErrorMessage = "Campo requerido")]
        public string nombre_cliente { get; set; }

        [MinLength(3, ErrorMessage = "Mínimo 3 caracteres")]
        [StringLength(15)]
        [Required(ErrorMessage = "Campo requerido")]
        public string apellido_cliente { get; set; }

        [StringLength(10)]
        [MinLength(10, ErrorMessage = "Se requieren 10 caracteres")]
        [Required(ErrorMessage = "Campo requerido")]
        public string telefono_cliente { get; set; }


        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [Required(ErrorMessage = "Campo requerido")]
        public string contrasena_nueva { get; set; }


        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [Required(ErrorMessage = "Campo requerido")]
        public string? confirmar_contrasena { get; set; }
    }
}
