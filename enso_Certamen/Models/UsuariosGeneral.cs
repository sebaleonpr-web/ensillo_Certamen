using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class UsuariosGeneral
{
    public Guid GuidUsuario { get; set; }

    public string ContraUser { get; set; } = null!;

    public string NombreUser { get; set; } = null!;

    public string ApellidoUser { get; set; } = null!;

    public string EmailUser { get; set; } = null!;

    public Guid? GuidRol { get; set; }

    public virtual RolGeneral? GuidRolNavigation { get; set; }

    public virtual ICollection<NoticiaGeneral> NoticiaGenerals { get; set; } = new List<NoticiaGeneral>();
}
