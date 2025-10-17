using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class SuscripcionGeneral
{
    public int IdSuscripcion { get; set; }

    public string NombreSuscripcion { get; set; } = null!;

    public string EmailSuscripcion { get; set; } = null!;

    public DateTime FechaSuscripcion { get; set; }

    public int IdBoletin { get; set; }
}
