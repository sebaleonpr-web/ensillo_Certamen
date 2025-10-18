using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class BoletinGeneral
{
    public string TituloBoletin { get; set; } = null!;

    public string DescripcionBoletin { get; set; } = null!;

    public DateTime FechaBoletin { get; set; }

    public int IdNoticia { get; set; }

    public int IdBoletin { get; set; }

    public Guid GuidBoletin { get; set; }

    public virtual NoticiaGeneral IdNoticiaNavigation { get; set; } = null!;
}
