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

    // YENİ POST METODU:
    [HttpPost]
    [HttpPost]
public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
{
    // Kart numarası boş değilse ve en az 16 haneliyse maskeleme yap
    // Örnek: 4321123456781234 -> 4321********1234
    if (!string.IsNullOrEmpty(transaction.CardNumber) && transaction.CardNumber.Length >= 16)
    {
        var first4 = transaction.CardNumber.Substring(0, 4);
        var last4 = transaction.CardNumber.Substring(transaction.CardNumber.Length - 4);
        
        transaction.CardNumber = $"{first4}********{last4}";
    }

    // Veritabanına maskelenmiş haliyle kaydedilir
    await _repository.AddAsync(transaction);
    return Ok(transaction);
}


    // YENİ GET METODU:
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(int id) // Veritabanı ID'leri genelde int olur
    {
        var transaction = await _repository.GetByIdAsync(id);
        
        if (transaction == null)
        {
            return NotFound("İşlem bulunamadı."); // Eğer veritabanında yoksa 404 döner
        }
        
        return Ok(transaction);
    }

    // YENİ GET METODU:
    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _repository.GetAllAsync();
        return Ok(transactions);
    }
}