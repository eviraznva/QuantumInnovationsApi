using QuantumInnovationsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace QuantumInnovationsApi.Context;

public class QuantumInnovationsDb : DbContext
{
    public DbSet<Raport> Raports { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=QuantumInnovations.db");
    }
}