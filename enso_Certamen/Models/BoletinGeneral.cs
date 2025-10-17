using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace enso_Certamen.Models
{
    public partial class BoletinGeneral
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBoletin { get; set; }

        [Required]
        public string TituloBoletin { get; set; } = null!;

        [Required]
        public string DescripcionBoletin { get; set; } = null!;

        [Required]
        public DateTime FechaBoletin { get; set; }

        public int? IdNoticia { get; set; }
    }
}
