using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

[Table("rolGeneral")]
public partial class rolGeneral
{
    [Key]
    public Guid GuidRol { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string nombreRol { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? descripRol { get; set; }

    [InverseProperty("GuidRolNavigation")]
    public virtual ICollection<usuariosGeneral> usuariosGenerals { get; set; } = new List<usuariosGeneral>();
}
