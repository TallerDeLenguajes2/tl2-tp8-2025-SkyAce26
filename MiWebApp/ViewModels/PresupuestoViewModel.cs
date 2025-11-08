using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace SistemaVentas.Web.ViewModels;

public class PresupuestoViewModel
{
    public int IdPresupuesto { get; set; }

    [Display(Name = "Nombre o Email del Destinatario.")]
    [Required(ErrorMessage = "El nombre o email es obligatorio.")]
    public string NombreDestinatario { get; set; }

    [Display(Name = "Fecha de creación")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; }
}