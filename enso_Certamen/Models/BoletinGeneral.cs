using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

[Table("boletinGeneral")]
public partial class boletinGeneral
{
    [Key]
    public Guid GuidBoletin { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [Required(ErrorMessage = "El Titulo es obligatorio.")]
    public string TituloBoletin { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    [Required(ErrorMessage = "La Descripcion es obligatorio.")]
    public string? DescripcionBoletin { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateOnly FechaBoletin { get; set; }

    public Guid? GuidNoticia { get; set; }

    [ForeignKey("GuidNoticia")]
    [InverseProperty("boletinGenerals")]
    public virtual noticiaGeneral? GuidNoticiaNavigation { get; set; }

    [InverseProperty("GuidBoletinNavigation")]
    public virtual ICollection<suscripcionGeneral> suscripcionGenerals { get; set; } = new List<suscripcionGeneral>();
}
