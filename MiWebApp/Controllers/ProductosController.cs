using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MiWebApp.Models;
using Models;
namespace MiWebApp.Controllers;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Producto> productos = productoRepository.GetAll();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Producto nuevoProducto)
    {
        if (ModelState.IsValid)
        {
            productoRepository.Alta(nuevoProducto);
            return RedirectToAction("Index");
        }

        return View(nuevoProducto);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var prod = productoRepository.Detalles(id);
        return View(prod);
    }

    [HttpPost]
    public IActionResult Edit(Producto producto)
    {
        productoRepository.ModificarProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int idProducto)
    {
        var prod = productoRepository.Detalles(idProducto);
        return View(prod);
    }

    [HttpPost]
     [ActionName("Delete")] 
    public IActionResult DeleteConfirmado(int idProducto)
    {
        productoRepository.Baja(idProducto);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
