namespace GitTransactionsService.Models;

public class Terminal
{
    public int Id { get; set; }
    public string MerchantId { get; set; } = string.Empty;
    public string TerminalNo { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}