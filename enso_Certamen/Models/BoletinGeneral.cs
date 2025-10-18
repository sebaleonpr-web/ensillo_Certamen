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


        [Display(Name = "Título")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 50 caracteres.")]
        public string TituloBoletin { get; set; } = null!;


        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string DescripcionBoletin { get; set; } = null!;


        [Display(Name = "Fecha del Boletín")]
        [Required(ErrorMessage = "La fecha del boletín es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaBoletin { get; set; }


        [Display(Name = "Noticia")]
        [Required(ErrorMessage = "Debes Seleccionar una noticia.")]
        public int? IdNoticia { get; set; }
    }
}
