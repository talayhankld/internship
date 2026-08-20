using System.ComponentModel;
namespace GitTransactionsService.Models;

public class StatusRequest
{
    [DefaultValue("MER-*********")]
    public string MerchantId { get; set; } = string.Empty;
    [DefaultValue("1")]
    public string TerminalNo { get; set; } = string.Empty;
}