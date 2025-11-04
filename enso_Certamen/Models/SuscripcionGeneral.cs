using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class SuscripcionGeneral
{
    public Guid GuidSuscripcion { get; set; }

    public string NombreSuscripcion { get; set; } = null!;

    public string EmailSuscripcion { get; set; } = null!;

    public DateTime FechaSuscripcion { get; set; }

    public Guid? GuidBoletin { get; set; }

    public virtual BoletinGeneral? GuidBoletinNavigation { get; set; }
}
