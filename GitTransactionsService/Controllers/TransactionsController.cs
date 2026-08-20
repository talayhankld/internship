using Microsoft.AspNetCore.Mvc;
using GitTransactionsService.Interfaces;
using GitTransactionsService.Models;
using System;
using System.Threading.Tasks;

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
            return BadRequest(new { Success = false, Message = "İşyeri adı boş olamaz." });
        
        if (string.IsNullOrEmpty(request.City))
            return BadRequest(new { Success = false, Message = "Şehir boş olamaz." });
        
        if (await _repository.CheckIfNameExistsAsync(request.Name))
            return BadRequest(new { Success = false, Message = "Bu isme sahip bir üye işyeri zaten mevcut." });

        var merchant = new Merchant
        {
            MerchantId = IdGenerator.GenerateMerchantId(),
            Name = request.Name,
            City = request.City,
            Status = MerchantStatus.Active,
            CreatedDate = DateTime.UtcNow
        };

        await _repository.CreateMerchantAsync(merchant);

        return Ok(new { Success = true, Message = "Merchant başarıyla oluşturuldu.", MerchantId = merchant.MerchantId });
    }

    [HttpGet("/api/merchants")]
    public async Task<IActionResult> GetAllMerchants()
    {
        var merchants = await _repository.GetAllMerchantsAsync();
        return Ok(merchants);
    }

    [HttpPost("/api/merchants/{merchantId}/terminals")]
    public async Task<IActionResult> CreateTerminal(string merchantId, [FromBody] CreateTerminal request)
    {
        if (string.IsNullOrEmpty(merchantId))
            return BadRequest(new { Success = false, Message = "Merchant ID boş olamaz." });
        
        if (string.IsNullOrEmpty(request.TerminalNo))
            return BadRequest(new { Success = false, Message = "Terminal numarası boş olamaz." });

        var merchant = await _repository.GetMerchantByIdAsync(merchantId);
        if (merchant == null)
            return BadRequest(new { Success = false, Message = "Bu üye işyeri mevcut değil." });
        
        if (merchant.Status != MerchantStatus.Active)
            return BadRequest(new { Success = false, Message = "Üye işyeri aktif değil. Terminal oluşturulamaz." });

        if (await _repository.CheckIfTerminalExistsAsync(merchantId, request.TerminalNo))
            return BadRequest(new { Success = false, Message = "Bu üye işyerine ait aynı numaralı bir terminal zaten mevcut." });
        
        if (string.IsNullOrEmpty(request.Currency)) 
            return BadRequest(new { Success = false, Message = "Para birimi boş olamaz." });
        
        string currencyCode = request.Currency.ToUpper();
        if(currencyCode != "USD" && currencyCode != "EUR" && currencyCode != "TRY")
            return BadRequest(new { Success = false, Message = "Geçersiz para birimi. Sadece USD, EUR veya TRY desteklenmektedir." });

        var terminal = new Terminal
        {
            MerchantId = merchantId,
            TerminalNo = request.TerminalNo,
            Currency = currencyCode,
            Status = TerminalStatus.Active,
            CreatedDate = DateTime.UtcNow
        };
    
        await _repository.AddTerminalAsync(terminal);

        return Ok(new { Success = true, Message = "Terminal başarıyla oluşturuldu.", TerminalNo = request.TerminalNo });
    }
    [HttpGet("/api/merchants/{merchantId}/terminals/list")]
    public async Task<IActionResult> GetTerminalsByMerchant([FromRoute] string merchantId)
    {
        var terminalsList = await _repository.GetTerminalsByMerchantIdAsync(merchantId);
        return Ok(terminalsList);
    }

    
    [HttpGet("/api/merchants/{merchantId}/status")]
    public async Task<IActionResult> CheckMerchantStatus(string merchantId)
    {
        if (string.IsNullOrEmpty(merchantId))
            return BadRequest(new { Success = false, Message = "Merchant ID boş olamaz." });

    bool isActive = await _repository.IsMerchantActiveAsync(merchantId);
    
        if (isActive)
            return Ok(new { Success = true, Message = "Üye işyeri aktif ve işlem yapmaya uygun." });
        else
            return BadRequest(new { Success = false, Message = "Üye işyeri bulunamadı veya aktif değil." });
    }

    [HttpGet("/api/merchants/{merchantId}/terminals/{terminalNo}/status")]
    public async Task<IActionResult> CheckTerminalStatus([FromRoute] string merchantId, [FromRoute] string terminalNo)
    {
        if (string.IsNullOrEmpty(merchantId))
            return BadRequest(new { Success = false, Message = "Merchant ID boş olamaz." });
                
        if (string.IsNullOrEmpty(terminalNo))
            return BadRequest(new { Success = false, Message = "Terminal numarası boş olamaz." });

       
        var terminal = await _repository.GetTerminalAsync(merchantId, terminalNo);
        
        if (terminal == null)
            return BadRequest(new { Success = false, Message = $"Hata: ID'si '{merchantId}'  olan işyerine ait '{terminalNo}' numaralı terminal sistemde bulunamadı." });

        if (terminal.Status != TerminalStatus.Active)
            return BadRequest(new { Success = false, Message = $"Hata: Terminal kapalı durumda. (Mevcut Durum: {terminal.Status})" });

        return Ok(new { Success = true, Message = "Terminal aktif ve işlem yapmaya uygun." });
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddTransaction([FromBody] CreateTransactionRequest request)
    {
        if (string.IsNullOrEmpty(request.MerchantId))
            return BadRequest(new { Success = false, Message = "Merchant ID boş olamaz." });
        
        if (string.IsNullOrEmpty(request.TerminalNo))
            return BadRequest(new { Success = false, Message = "Terminal numarası boş olamaz." });
        
        if (string.IsNullOrEmpty(request.Currency)) 
            return BadRequest(new { Success = false, Message = "Para birimi boş olamaz." });
        
        if (request.Amount <= 0 || request.Amount > 1000000)
            return BadRequest(new { Success = false, Message = "Geçersiz tutar. İşlem tutarı en fazla 1.000.000 olabilir." });
        
        string currencyCode = request.Currency.ToUpper();
        if(currencyCode != "USD" && currencyCode != "EUR" && currencyCode != "TRY")
            return BadRequest(new { Success = false, Message = "Geçersiz para birimi. Sadece USD, EUR veya TRY desteklenmektedir." });

        var merchant = await _repository.GetMerchantByIdAsync(request.MerchantId);
        if (merchant == null)
            return BadRequest(new { Success = false, Message = $"Hata: '{request.MerchantId}' numaralı üye işyeri sistemde bulunamadı." });

        var terminal = await _repository.GetTerminalAsync(request.MerchantId, request.TerminalNo); 
        if (terminal == null)
            return BadRequest(new { Success = false, Message = $"Hata: '{request.TerminalNo}' numaralı terminal sistemde bulunamadı." });
        
        if (terminal.Currency != currencyCode)
            return BadRequest(new { Success = false, Message = $"Hata: '{request.TerminalNo}' numaralı terminalin para birimi '{currencyCode}' ile eşleşmiyor." });

        if (string.IsNullOrEmpty(request.CardNumber) || request.CardNumber.Length != 16)
            return BadRequest(new { Success = false, Message = "Kart numarası geçersiz. Lütfen tam 16 haneli bir kart numarası giriniz." });
        
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

        return Ok(new { ReferenceNumber = transaction.ReferenceNumber, Amount = transaction.Amount, Status = transaction.Status });
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelTransaction([FromBody] CancelTransactionRequest request)
    {
        if (string.IsNullOrEmpty(request.ReferenceNumber))
            return BadRequest(new { Success = false, Message = "Referans numarası boş olamaz." });

        var transaction = await _repository.GetByRefNumberAsync(request.ReferenceNumber);
        if (transaction == null)
            return BadRequest(new { Success = false, Message = "İşlem bulunamadı." });

        decimal currentAmount = transaction.Amount - transaction.CancelAmount;
        
        if (request.CancelAmount <= 0)
            return BadRequest(new { Success = false, Message = "İptal edilecek tutar sıfır veya negatif olamaz." });

        if (request.CancelAmount > currentAmount)
            return BadRequest(new { Success = false, Message = "İptal edilecek tutar mevcut tutardan fazla olamaz." });
        
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

        return Ok(new { Success = true, Message = "İşlem tamamlandı.", RefNumber = transaction.ReferenceNumber, Status = transaction.Status });
    }

    [HttpGet("ReferenceNumber")] 
    public async Task<IActionResult> GetByRefNumberAsync(string referenceNumber)
    {
        var transaction = await _repository.GetByRefNumberAsync(referenceNumber);
        if (transaction == null)
            return BadRequest(new { Success = false, Message = "İşlem bulunamadı." }); 
        
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