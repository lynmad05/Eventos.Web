using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Data;

public class EventosContext : DbContext
{
    public EventosContext(DbContextOptions<EventosContext> options) : base(options) { }

    public virtual DbSet<Categoria> Categorias { get; set; }
    public virtual DbSet<Evento> Eventos { get; set; }
    public virtual DbSet<Asistente> Asistentes { get; set; }
    public virtual DbSet<Inscripcion> Inscripciones { get; set; }
}