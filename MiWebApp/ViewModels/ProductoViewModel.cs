using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace SistemaVentas.Web.ViewModels;

public class ProductoViewModel
{
    public int IdProducto { get; set; }

    [Display(Name = "Descripción del producto")]
    [StringLength(250, ErrorMessage = "El precio es obligatorio.")]
    public string Descripcion { get; set; }

    [Display(Name = "Precio Unitario")]
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public decimal Precio { get; set; }
}