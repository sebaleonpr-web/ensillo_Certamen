using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class RolGeneral
{
    public Guid GuidRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public string DescripRol { get; set; } = null!;

    public virtual ICollection<UsuariosGeneral> UsuariosGenerals { get; set; } = new List<UsuariosGeneral>();
}
