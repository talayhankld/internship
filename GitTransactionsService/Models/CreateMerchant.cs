using System.ComponentModel;
namespace GitTransactionsService.Models;

public class CreateMerchant
{
    [DefaultValue("Provision")]
    public string Name { get; set; } = string.Empty;

    [DefaultValue("Istanbul")]
    public string City { get; set; } = string.Empty;

    [DefaultValue("Active")]
    public string Status { get; set; } = string.Empty;
}