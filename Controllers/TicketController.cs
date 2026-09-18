using AtiendiTicketsAPI.Datos;
using AtiendiTicketsAPI.Entidades;
using AtiendiTicketsAPI.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtiendiTicketsAPI.Controllers;

/// <summary>
/// Controlador para manejar las operaciones relacionadas con los tickets.
/// </summary>
[ApiController]
[Route("api/tickets")]
public class TicketController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor que recibe el contexto de la base de datos
    /// a través de inyección de dependencias.
    /// </summary>
    /// <param name="context"></param>
    public TicketController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los tickets de la base de datos y los devuelve ordenados por fecha de creación.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        var tickets = await _context.Tickets
            .OrderBy(t => t.FechaCreacion)
            .ToListAsync();
        return Ok(tickets);
    }

    /// <summary>
    /// Obtiene un ticket específico por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(Guid id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        return Ok(ticket);
    }

    /// <summary>
    /// Crea un nuevo ticket.
    /// </summary>
    /// <param name="ticket"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
    {
        if(string.IsNullOrWhiteSpace(ticket.Cliente) || string.IsNullOrWhiteSpace(ticket.Asunto) || string.IsNullOrWhiteSpace(ticket.Descripcion))
        {
            return BadRequest("Los campos Cliente, Asunto y Descripción son obligatorios.");
        }
        if (!EsPrioridadValida(ticket.Prioridad))
        {
            return BadRequest("La prioridad del ticket debe ser 'Baja', 'Media', 'Alta' o 'Crítica'.");
        }

        if (ticket.Estado != Estado.Abierto)
        {
            return BadRequest("El estado inicial del ticket debe ser 'Abierto'.");
        }
        
        // Para evitar crear DTOs y mantener la simplicidad del código,
        // se puede crear un nuevo objeto Ticket y asignar los valores
        // del ticket recibido en la petición para controlar su creación.
        var peticionCreacionTicket = new Ticket
        {
            // El cliente no puede enviar el ID, ya que este se genera automáticamente en la entidad Ticket.
            Cliente = ticket.Cliente,
            Asunto = ticket.Asunto,
            Descripcion = ticket.Descripcion,
            Prioridad = ticket.Prioridad,
            Estado = ticket.Estado,
            // Tampoco se permite que el cliente envíe la fecha de creación ya que esta se genera automáticamente en la entidad Ticket.
        };
        
        await _context.Tickets.AddAsync(peticionCreacionTicket);
        await _context.SaveChangesAsync();

        // Retornamos un codigo 201 Created con la ubicación del nuevo ticket creado.
        return CreatedAtAction(
            nameof(GetTicketById),
            new { id = peticionCreacionTicket.Id },
            peticionCreacionTicket);    
    }

    /// <summary>
    /// Actualiza un ticket existente.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ticket"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] Ticket ticket)
    {
        var existingTicket = await _context.Tickets.FindAsync(id);
        if (existingTicket == null)
        {
            return NotFound();
        }
        if(!EsPrioridadValida(ticket.Prioridad))
        {
            return BadRequest("La prioridad del ticket debe ser 'Baja', 'Media', 'Alta' o 'Crítica'.");
        }
        if(!EsEstadoValido(ticket.Estado))
        {
            return BadRequest("El estado del ticket debe ser 'Abierto', 'En Progreso', 'Resuelto' o 'Cancelado'.");
        }

        existingTicket.Cliente = ticket.Cliente;
        existingTicket.Asunto = ticket.Asunto;
        existingTicket.Descripcion = ticket.Descripcion;
        existingTicket.Prioridad = ticket.Prioridad;
        existingTicket.Estado = ticket.Estado;

        await _context.SaveChangesAsync();
        
        return Ok(existingTicket);
    }
    
    /// <summary>
    /// Elimina un ticket existente.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var existingTicket = await _context.Tickets.FindAsync(id);
        if (existingTicket == null)
        {
            return NotFound();
        }

        _context.Tickets.Remove(existingTicket);
        await _context.SaveChangesAsync();
        return NoContent();
    }


    /// <summary>
    /// Valida que la prioridad del ticket sea una de las opciones válidas del enum Prioridad.
    /// </summary>
    /// <param name="prioridad"></param>
    /// <returns></returns>
    private static bool EsPrioridadValida(Prioridad prioridad)
    {
        return System.Enum.IsDefined(typeof(Prioridad), prioridad);
    }
    
    /// <summary>
    /// Valida que el estado del ticket sea una de las opciones válidas del enum Estado.
    /// </summary>
    /// <param name="estado"></param>
    /// <returns></returns>
    private static bool EsEstadoValido(Estado estado)
    {
        return System.Enum.IsDefined(typeof(Estado), estado);
    }
}

