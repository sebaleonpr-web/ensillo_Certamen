using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class UsuariosGeneral
{
    public int IdUser { get; set; }

    public string ContraUser { get; set; } = null!;

    public string NombreUser { get; set; } = null!;

    public string ApellidoUser { get; set; } = null!;

    public string EmailUser { get; set; } = null!;

    public int IdRol { get; set; }

    public Guid GuidUsuario { get; set; }
}
