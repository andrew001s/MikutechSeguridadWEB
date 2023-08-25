using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Protov4.DTO
{
    public class UsuariosDTO
    {
        [Key]
        public int id_usuario { get; set; }

        [EmailAddress]
        [StringLength(50)]
        [Required(ErrorMessage = "Campo requerido")]
        public string? correo_elec { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string? contrasena { get; set; }

        public int id_rol_user { get; set; }

    }
}
