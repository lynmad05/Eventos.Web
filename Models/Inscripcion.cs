using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventos.Web.Models;

public class Inscripcion
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Evento")]
    public int EventoId { get; set; }

    [Display(Name = "Asistente")]
    public int AsistenteId { get; set; }

    [Display(Name = "Fecha de inscripción")]
    public DateTime FechaInscripcion { get; set; }

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Confirmada";

    [ForeignKey(nameof(EventoId))]
    public virtual Evento? Evento { get; set; }

    [ForeignKey(nameof(AsistenteId))]
    public virtual Asistente? Asistente { get; set; }
}