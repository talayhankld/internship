using Microsoft.AspNetCore.Mvc;
using GitTransactionsService.Interfaces;
using GitTransactionsService.Models;

namespace GitTransactionsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionRepository _repository;

    public TransactionsController(ITransactionRepository repository)
    {
        _repository = repository;
    }
    [HttpPost("/api/merchants/add")]
    public async Task<IActionResult> CreateMerchant([FromBody] CreateMerchant request)
    {
        if (string.IsNullOrEmpty(request.Name)) 
        {
            return BadRequest(new { Success = false, Message = "Adı boş olamaz." });
        }
        bool isDuplicate = await _repository.CheckIfNameExistsAsync(request.Name);

        if (isDuplicate)
        {
            return BadRequest(new { Success = false, Message = "Bu isme sahip bir üye işyeri zaten mevcut." });
        }
        if (string.IsNullOrEmpty(request.City))
        {
            return BadRequest(new { Success = false, Message = "Şehir boş olamaz." });
        }
        

        var merchant = new Merchant
        {
            MerchantId = IdGenerator.GenerateMerchantId(),
            Name = request.Name,
            City = request.City,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow
        };

        await _repository.CreateMerchantAsync(merchant);

        return Ok(new
        {
            Success = true,
            Message = "Merchant başarıyla oluşturuldu.",
            MerchantId = merchant.MerchantId
        });
    }

    [HttpPost("/api/merchants/{merchantId}/terminals")]
    public async Task<IActionResult> CreateTerminal(string merchantId, [FromBody] CreateTerminal request)
    {
        if (string.IsNullOrEmpty(request.TerminalNo))
        {
            return BadRequest(new { Success = false, Message = "Terminal numarası boş olamaz." });
        }
            bool isTerminalExists = await _repository.CheckIfTerminalExistsAsync(merchantId, request.TerminalNo);

        if (isTerminalExists)
        {
            return BadRequest(new { Success = false, Message = "Bu üye işyerine  ait aynı numaralı bir terminal zaten mevcut." });
        }
        if (string.IsNullOrEmpty(request.Currency)) 
        {
            return BadRequest(new { Success = false, Message = "Para birimi boş olamaz." });
        }
        string currencyCode = request.Currency.ToUpper();
        if(currencyCode != "USD" && currencyCode != "EUR" && currencyCode != "TRY")
        {
            return BadRequest(new { Success = false, Message = "Geçersiz para birimi. Sadece USD, EUR veya TRY desteklenmektedir." });
        }

        var terminal = new Terminal
        {
            MerchantId = merchantId,
            TerminalNo = request.TerminalNo,
            Currency = request.Currency,
            Status = "Active",
            CreatedDate = DateTime.UtcNow
        };

        await _repository.AddTerminalAsync(terminal);

        return Ok(new
        {
            Success = true,
            Message = "Terminal başarıyla oluşturuldu.",
            TerminalId = terminal.Id
        });
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTransaction([FromBody] CreateTransactionRequest request)
    {
        if (string.IsNullOrEmpty(request.MerchantId))
    {
        return BadRequest(new 
        { 
            Success = false, 
            Message = "Merchant Id boş olamaz." 
        });
    }
        if (string.IsNullOrEmpty(request.TerminalNo))
    {
        return BadRequest(new 
        { 
            Success = false, 
            Message = "Terminal No boş olamaz." 
        });
        
    }
        if (string.IsNullOrEmpty(request.Currency)) 
    {
        return BadRequest(new 
        { 
            Success = false, Message = "Para birimi boş olamaz." 
        });
    }
        if (request.Amount <= 0 || request.Amount > 1000000)
    {
        return BadRequest(new 
        { 
            Success = false, 
            Message = "Geçersiz tutar. İşlem tutarı 0'dan büyük ve en fazla 1.000.000 olabilir." 
        });
    }
        string currencyCode = request.Currency.ToUpper();
        
        if(currencyCode != "USD" && currencyCode != "EUR" && currencyCode != "TRY")
    {
        
        return BadRequest(new 
        { 
            Success = false, 
            Message = "Geçersiz para birimi. Sadece USD, EUR veya TRY desteklenmektedir." 
        });
    }
    

    bool isTerminalValid = await _repository.IsTerminalValidAsync(request.TerminalNo);
    
    if (!isTerminalValid)
    {
        return BadRequest(new 
        { 
            Success = false, 
            Message = $"Hata: '{request.TerminalNo}' numaralı terminal sistemde bulunamadı." 
        });
    }
    bool isMerchantValid = await _repository.CheckIfMerchantExistsAsync(request.MerchantId);

    if (!isMerchantValid)
    {
        return BadRequest(new 
        { 
            Success = false, 
            Message = $"Hata: '{request.MerchantId}' numaralı üye işyeri sistemde bulunamadı." 
        });
    }
    if (string.IsNullOrEmpty(request.CardNumber) || request.CardNumber.Length != 16)
    {
    return BadRequest(new 
    { 
        Success = false, 
        Message = "Kart numarası geçersiz. Lütfen tam 16 haneli bir kart numarası giriniz." 
    });
    }
        
            var first8 = request.CardNumber.Substring(0, 8);
            var last4 = request.CardNumber.Substring(request.CardNumber.Length - 4);
            string maskedCard = $"{first8}****{last4}";
        

        var transaction = new Transaction
        {
            ReferenceNumber = IdGenerator.GenerateTransactionRef(),
            TerminalNo = request.TerminalNo,
            CardNumber = maskedCard,
            Amount = request.Amount,
            Currency = currencyCode,
            Status = "Pending", 
            CancellationReason = null,
            CancelledAt = null, 
        };
        
        await _repository.AddAsync(transaction);

        return Ok(new 
        {
            ReferenceNumber = transaction.ReferenceNumber,
            Amount = transaction.Amount,
            Status = transaction.Status 
        });
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelTransaction([FromBody] CancelTransactionRequest request)
    {
        if (string.IsNullOrEmpty(request.ReferenceNumber))
        {
            return BadRequest(new { Success = false, Message = "Referans numarası boş olamaz." });
        }

        var transaction = await _repository.GetByRefNumberAsync(request.ReferenceNumber);

        
        if (transaction == null)
        {
            return BadRequest(new { Success = false, Message = "İşlem bulunamadı." });
        }

        decimal currentAmount = transaction.Amount - transaction.CancelAmount;
        if (request.CancelAmount > currentAmount)
        {
            return BadRequest(new { Success = false, Message = "İptal edilecek tutar mevcut tutardan fazla olamaz." });
        }
        if (request.CancelAmount <= 0)
        {
            return BadRequest(new { Success = false, Message = "İptal edilecek tutar sıfır veya negatif olamaz." });
        }
        
        if (request.CancelAmount == currentAmount)
        {
            transaction.Status = "Cancelled";
            transaction.CancelAmount += request.CancelAmount;
        }
        else
        {
            transaction.Status = "Partially Cancelled";
            transaction.CancelAmount += request.CancelAmount;
        }

        await _repository.UpdateAsync(transaction); 

        return Ok(new 
        {
            Success = true,
            Message = "İşlem başarıyla iptal edildi.",
            RefNumber = transaction.ReferenceNumber,
            Status = transaction.Status
        });
    }
    [HttpGet("ReferenceNumber")] 
    public async Task<IActionResult> GetByRefNumberAsync(string referenceNumber)
    {
        var transaction = await _repository.GetByRefNumberAsync(referenceNumber);
        if (transaction == null)
        {
            return BadRequest("İşlem bulunamadı.");
        }
        
        decimal currentAmount = transaction.Amount - transaction.CancelAmount;
        return Ok(new
        {
            ReferenceNumber = transaction.ReferenceNumber,
            Amount = transaction.Amount,
            CurrentAmount = currentAmount,
            Status = transaction.Status
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _repository.GetAllAsync();
        return Ok(transactions);
    }
}