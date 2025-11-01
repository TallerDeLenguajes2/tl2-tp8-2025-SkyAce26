using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiWebApp.Models;
using espacioProductoRepository;
using espacioProducto;
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
