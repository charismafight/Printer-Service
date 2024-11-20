using Microsoft.EntityFrameworkCore;

namespace PrinterService.Models;

public class PrinterContext : DbContext
{
    public PrinterContext(DbContextOptions<PrinterContext> options) : base(options)
    {

    }

    public DbSet<PrintRecord> PrintRecords { get; set; } = null!;
}
