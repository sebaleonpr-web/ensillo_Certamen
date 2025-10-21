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
    [Required(ErrorMessage = "El Titulo es obligatorio.")]

    public string tituloNoticia { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
        [Required(ErrorMessage = "El Resumen es obligatorio.")]

    public string? resumenNoticia { get; set; }

    [Column(TypeName = "text")]
        [Required(ErrorMessage = "El Contenido es obligatorio.")]

    public string? contenidoNoticia { get; set; }

    [Display(Name = "Ingrese fecha")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "La Fecha es obligatoria.")]
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
