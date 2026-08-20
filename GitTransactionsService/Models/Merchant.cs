namespace GitTransactionsService.Models;

public class Merchant
{
    public int Id { get; set; }
    public string MerchantId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public MerchantStatus Status { get; set; } = MerchantStatus.Active;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}