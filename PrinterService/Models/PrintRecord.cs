public class PrintRecord
{
    public long Id { get; set; }

    public required string FileName { get; set; }

    public required string FileLocation { get; set; }

    public string? IP { get; set; }

    public DateTime PrintTime { get; set; }
}
