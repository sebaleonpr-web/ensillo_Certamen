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

    public string nombreSuscripcion { get; set; } = null!;

    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    public string emailSuscripcion { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime fechaSuscripcion { get; set; }

    public Guid? GuidBoletin { get; set; }

    [ForeignKey("GuidBoletin")]
    [InverseProperty("suscripcionGenerals")]
    public virtual boletinGeneral? GuidBoletinNavigation { get; set; }
}
