using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

[Table("comentarioGeneral")]
public partial class comentarioGeneral
{
    [Key]
    public Guid GuidComentario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50)]
    [Unicode(false)]
    public string nombrelectorComentario { get; set; } = null!;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [StringLength(50)]
    [Unicode(false)]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string emailLectorComentario { get; set; } = null!;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    [Column(TypeName = "text")]
    public string contenidoComentario { get; set; } = null!;

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [Column(TypeName = "datetime")]
    public DateTime fechaComentario { get; set; }

    public Guid? GuidNoticia { get; set; }

    [ForeignKey("GuidNoticia")]
    [InverseProperty("comentarioGenerals")]
    public virtual noticiaGeneral? GuidNoticiaNavigation { get; set; } // ← nullable
}
