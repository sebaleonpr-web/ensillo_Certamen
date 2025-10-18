using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

[Table("suscripcionGeneral")]
public partial class suscripcionGeneral
{
    [Key]
    public Guid GuidSuscripcion { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string nombreSuscripcion { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string emailSuscripcion { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime fechaSuscripcion { get; set; }

    public Guid GuidBoletin { get; set; }

    [ForeignKey("GuidBoletin")]
    [InverseProperty("suscripcionGenerals")]
    public virtual boletinGeneral GuidBoletinNavigation { get; set; } = null!;
}
