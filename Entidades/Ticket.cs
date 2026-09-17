using System.ComponentModel.DataAnnotations;
using AtiendiTicketsAPI.Enum;

namespace AtiendiTicketsAPI.Entidades;
/// <summary>
/// Entidad Ticket que representa un ticket de soporte en el sistema.
/// </summary>
public class Ticket
{
    /// <summary>
    /// El ID único del ticket asignado por el sistema.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Cliente { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Asunto { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Descripcion { get; set; }
    
    /// <summary>
    /// Representa la prioridad del ticket, que puede ser Baja, Media, Alta o Crítica.
    /// Utiliza ENUM para definir los posibles valores de prioridad.
    /// </summary>
    public Prioridad Prioridad { get; set; }
    
    /// <summary>
    /// Representa el estado actual del ticket, que puede ser:
    /// Abierto, En Progreso, Resuelto o Cancelado.
    /// Utiliza ENUM para definir los posibles valores de estado.
    /// </summary>
    public Estado Estado { get; set; }
    
    /// <summary>
    /// La fecha y hora en que se creó el ticket.
    /// Se establece automáticamente al crear un nuevo ticket.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}