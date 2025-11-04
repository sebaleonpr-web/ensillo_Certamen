using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class BoletinGeneral
{
    public Guid GuidBoletin { get; set; }

    public string TituloBoletin { get; set; } = null!;

    public string DescripcionBoletin { get; set; } = null!;

    public DateTime FechaBoletin { get; set; }

    public Guid? GuidNoticia { get; set; }

    public virtual NoticiaGeneral? GuidNoticiaNavigation { get; set; }

    public virtual ICollection<SuscripcionGeneral> SuscripcionGenerals { get; set; } = new List<SuscripcionGeneral>();
}
