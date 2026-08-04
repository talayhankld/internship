
namespace GitTransactionsService.Models;
using System.ComponentModel;


public class Transaction
{

    public int Id { get; set; } 

    [DefaultValue(1)]
    public int TerminalId { get; set; }

    [DefaultValue(150.50)]
    public decimal Amount { get; set; }

    [DefaultValue("TRY")]
    public string Currency { get; set; }

    [DefaultValue("Success")]
    public string Status { get; set; }

    [DefaultValue("4321123456781234")]
    public string CardNumber { get; set; }

    [DefaultValue("REF987654321")]
    public string ReferenceNumber { get; set; }
    
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}