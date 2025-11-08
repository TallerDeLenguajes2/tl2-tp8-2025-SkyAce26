using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiWebApp.Models;
using Repositories;
using Models;
namespace MiWebApp.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuesto> presupuestos = presupuestoRepository.GetAll();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Presupuesto());
    }

    [HttpPost]
    public IActionResult Create(Presupuesto presupuesto)
    {
        presupuesto.PresupuestoDetalle = [];
        presupuestoRepository.Alta(presupuesto);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var presupuesto = presupuestoRepository.GetDetalles(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Presupuesto buscado = presupuestoRepository.GetDetalles(id);
        return View(buscado);
    }

    public IActionResult Edit(Presupuesto presupuesto)
    {
        presupuestoRepository.ActualizarPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Presupuesto buscado = presupuestoRepository.GetDetalles(id);
        return View(buscado);
    }

    [HttpPost]
    public IActionResult DeleteConfirmado(int id)
    {
        presupuestoRepository.Eliminar(id);
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}