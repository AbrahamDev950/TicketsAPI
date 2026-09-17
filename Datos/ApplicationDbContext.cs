using Microsoft.EntityFrameworkCore;
using AtiendiTicketsAPI.Entidades;

namespace AtiendiTicketsAPI.Datos;

/// <summary>
/// Representa el contexto de la base de datos para la aplicación.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Constructor que recibe las opciones de configuración del contexto de la base de datos.
    /// </summary>
    /// <param name="options"></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Seccion para definir las entidades de la base de datos
    public DbSet<Ticket> Tickets { get; set; }
}