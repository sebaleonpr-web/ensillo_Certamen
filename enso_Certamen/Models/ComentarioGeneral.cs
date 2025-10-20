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

    [StringLength(50)]
    [Unicode(false)]
    public string nombrelectorComentario { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string emailLectorComentario { get; set; } = null!;

    [Column(TypeName = "text")]
    public string contenidoComentario { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime fechaComentario { get; set; }


    public Guid? GuidNoticia { get; set; }

    [ForeignKey("GuidNoticia")]
    [InverseProperty("comentarioGenerals")]
    public virtual noticiaGeneral GuidNoticiaNavigation { get; set; } = null!;
}
