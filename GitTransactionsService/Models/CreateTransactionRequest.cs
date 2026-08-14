using System.ComponentModel;
namespace GitTransactionsService.Models;

public class CreateTransactionRequest
{
    [DefaultValue("1")]

    public string MerchantId { get; set; } = string.Empty;
   
    public string TerminalNo { get; set; } = string.Empty;
    
    [DefaultValue("4321123456781234")]
    public string CardNumber { get; set; } = string.Empty;

     [DefaultValue("0")]

    public decimal Amount { get; set; } = decimal.Zero;

     [DefaultValue("TRY")]

    public string Currency { get; set; } = string.Empty;
    
}