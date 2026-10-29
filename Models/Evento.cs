using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventos.Web.Models;

public class Evento
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "El lugar es obligatorio.")]
    [StringLength(100)]
    public string Lugar { get; set; } = string.Empty;

    [Range(1, 5000, ErrorMessage = "El aforo debe estar entre 1 y 5000.")]
    public int Aforo { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(0, 99999999.99, ErrorMessage = "El precio no puede ser negativo.")]
    public decimal Precio { get; set; }

    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    public bool Activo { get; set; } = true;

    [ForeignKey(nameof(CategoriaId))]
    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}