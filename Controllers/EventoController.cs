using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.Controllers;

public class EventosController : Controller
{
    private readonly EventoRepositorio _repo;

    public EventosController(EventoRepositorio repo) => _repo = repo;

    public async Task<IActionResult> Index(int? categoriaId)
    {
        var categorias = await _repo.ListarCategoriasActivasAsync();
        ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", categoriaId);
        return View(await _repo.ListarAsync(categoriaId));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var evento = await _repo.ObtenerPorIdAsync(id.Value);
        if (evento == null) return NotFound();
        return View(evento);
    }

    public async Task<IActionResult> Create()
    {
        await CargarCategoriasAsync();
        return View(new Evento { Fecha = DateTime.Today.AddDays(7).AddHours(18) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Evento evento)
    {
        if (ModelState.IsValid)
        {
            evento.Activo = true;
            await _repo.CrearAsync(evento);
            TempData["Mensaje"] = "Evento creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategoriasAsync(evento.CategoriaId);
        return View(evento);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var evento = await _repo.ObtenerPorIdAsync(id.Value);
        if (evento == null) return NotFound();

        await CargarCategoriasAsync(evento.CategoriaId);
        return View(evento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Evento evento)
    {
        if (ModelState.IsValid)
        {
            await _repo.ActualizarAsync(evento);
            TempData["Mensaje"] = "Evento actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategoriasAsync(evento.CategoriaId);
        return View(evento);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var evento = await _repo.ObtenerPorIdAsync(id.Value);
        if (evento == null) return NotFound();
        return View(evento);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ok = await _repo.EliminarLogicoAsync(id);
        TempData["Mensaje"] = ok ? "Evento eliminado correctamente." : "No se encontró el evento.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCategoriasAsync(int? seleccionada = null)
    {
        var categorias = await _repo.ListarCategoriasActivasAsync();
        ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", seleccionada);
    }
}