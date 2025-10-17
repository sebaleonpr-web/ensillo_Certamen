using System;
using System.Collections.Generic;

namespace enso_Certamen.Models;

public partial class ComentarioGeneral
{
    public int IdComentario { get; set; }

    public string NombrelectorComentario { get; set; } = null!;

    public string EmailLectorComentario { get; set; } = null!;

    public string ContenidoComentario { get; set; } = null!;

    public DateTime FechaComentario { get; set; }

    public int IdNoticia { get; set; }
}
