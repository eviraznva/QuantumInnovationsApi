namespace QuantumInnovationsApi.Models;

public class Raport
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public DateOnly Date { get; set; }
    public string Author { get; set; } = "";
    public string Content { get; set; } = "";
    public IEnumerable<string> Details { get; set; } = new List<string>();
    public string Conclusions { get; set; } = "";
}