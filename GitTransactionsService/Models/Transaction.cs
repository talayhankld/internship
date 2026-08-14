using System.ComponentModel;
using System.Text.Json.Serialization;

namespace GitTransactionsService.Models;


public class Transaction
{
    [JsonIgnore]
    public int Id { get; set; }
    
    [DefaultValue("REF987654321")]
    public string ReferenceNumber { get; set; } = string.Empty;
    
    [DefaultValue("4321123456781234")]
    public string CardNumber { get; set; } = string.Empty;

    [DefaultValue("1")]
    public string TerminalNo { get; set; } = string.Empty;

    [DefaultValue(150.50)]
    public decimal Amount { get; set; } = decimal.Zero;

    [DefaultValue("TRY")]
    public string Currency { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal CancelAmount { get; set; } = decimal.Zero;

    public decimal CurrentAmount { get; set; } = decimal.Zero;

    public DateTime? CancelledAt { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}