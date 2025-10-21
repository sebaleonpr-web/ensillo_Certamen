using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

[Table("usuariosGeneral")]
public partial class usuariosGeneral
{
    [Key]
    public Guid GuidUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
        [Required(ErrorMessage = "La Contraseña es obligatorio.")]

    public string contraUser { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
        [Required(ErrorMessage = "El Nombre es obligatorio.")]

    public string nombreUser { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
        [Required(ErrorMessage = "El Apellido es obligatorio.")]

    public string apellidoUser { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
        [Required(ErrorMessage = "El Correo es obligatorio.")]

    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    public string emailUser { get; set; } = null!;

    public Guid? GuidRol { get; set; } 

        [ForeignKey("GuidRol")]
        [InverseProperty("usuariosGenerals")]
        public virtual rolGeneral? GuidRolNavigation { get; set; }

    [InverseProperty("GuidUsuarioNavigation")]
    public virtual ICollection<noticiaGeneral> noticiaGenerals { get; set; } = new List<noticiaGeneral>();
}
