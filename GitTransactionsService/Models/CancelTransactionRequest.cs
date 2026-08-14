using System.ComponentModel;
namespace GitTransactionsService.Models;

public class CancelTransactionRequest
{

    [DefaultValue("REF987654321")]

    public required string ReferenceNumber { get; set; } 

     [DefaultValue("0")]

    public decimal CancelAmount { get; set; }

     [DefaultValue("Vazgeçildi")]

    public string? CancellationReason { get; set; }
    
}