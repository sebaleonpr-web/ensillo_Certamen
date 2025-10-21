using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

[Table("noticiaGeneral")]
public partial class noticiaGeneral
{
    [Key]
    public Guid GuidNoticia { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string tituloNoticia { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? resumenNoticia { get; set; }

    [Column(TypeName = "text")]
    public string? contenidoNoticia { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime fechaNoticia { get; set; }

    public Guid? GuidUsuario { get; set; }

    [ForeignKey("GuidUsuario")]
    [InverseProperty("noticiaGenerals")]
    public virtual usuariosGeneral? GuidUsuarioNavigation { get; set; }

    [InverseProperty("GuidNoticiaNavigation")]
    public virtual ICollection<boletinGeneral> boletinGenerals { get; set; } = new List<boletinGeneral>();

    [InverseProperty("GuidNoticiaNavigation")]
    public virtual ICollection<comentarioGeneral> comentarioGenerals { get; set; } = new List<comentarioGeneral>();
}
