namespace GitTransactionsService.Models;

public class Terminal
{
    public int Id { get; set; }
    public string MerchantId { get; set; } = string.Empty;
    public string TerminalNo { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public TerminalStatus Status { get; set; } = TerminalStatus.Active;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}