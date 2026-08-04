namespace GitTransactionsService.Models;

public class Terminal
{
    public int Id { get; set; }
    public int MerchantId { get; set; }
    public string TerminalNo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}