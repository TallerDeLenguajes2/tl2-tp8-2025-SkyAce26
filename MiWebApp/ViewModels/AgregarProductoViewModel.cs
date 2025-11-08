using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaVentas.Web.ViewModels;

public class AgregarProductoViewModel
{
    public int IdPresupuesto { get; set; }

    [Display(Name = "Producto a agregar")]
    public int IdProducto { get; set; }

    [Display(Name = "Cantidad")]
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "El cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }

    public SelectList ListaProductos { get; set; }
}