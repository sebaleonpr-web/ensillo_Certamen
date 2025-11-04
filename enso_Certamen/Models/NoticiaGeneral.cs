using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class NoticiaGeneral
{
    public Guid GuidNoticia { get; set; }

    public string TituloNoticia { get; set; } = null!;

    public string ResumenNoticia { get; set; } = null!;

    public string ContenidoNoticia { get; set; } = null!;

    public DateTime FechaNoticia { get; set; }

    public Guid? GuidUsuario { get; set; }

    public virtual ICollection<BoletinGeneral> BoletinGenerals { get; set; } = new List<BoletinGeneral>();

    public virtual ICollection<ComentarioGeneral> ComentarioGenerals { get; set; } = new List<ComentarioGeneral>();

    public virtual UsuariosGeneral? GuidUsuarioNavigation { get; set; }
}
