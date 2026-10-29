using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios;

public class EventoRepositorio
{
    private readonly EventosContext _context;

    public EventoRepositorio(EventosContext context) => _context = context;

    public async Task<List<Evento>> ListarAsync(int? categoriaId)
    {
        IQueryable<Evento> query = _context.Eventos
            .AsNoTracking()
            .Where(e => e.Activo)
            .Include(e => e.Categoria)
            .Include(e => e.Inscripciones.Where(i => i.Estado == "Confirmada"));

        if (categoriaId.HasValue)
            query = query.Where(e => e.CategoriaId == categoriaId.Value);

        return await query.OrderBy(e => e.Fecha).ToListAsync();
    }

    public async Task<Evento?> ObtenerPorIdAsync(int id)
    {
        return await _context.Eventos
            .AsNoTracking()
            .Include(e => e.Categoria)
            .Include(e => e.Inscripciones.Where(i => i.Estado == "Confirmada"))
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    }

    public async Task<List<Categoria>> ListarCategoriasActivasAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task CrearAsync(Evento evento)
    {
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Evento evento)
    {
        _context.Eventos.Update(evento);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) return false;

        evento.Activo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}