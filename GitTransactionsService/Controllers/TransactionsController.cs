using Microsoft.AspNetCore.Mvc;
using GitTransactionsService.Interfaces;

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

    [HttpPost]
    public IActionResult AddTransaction(string id, string message)
    {
        _repository.AddTransaction(id, message);
        return Ok("Transaction added");
    }

    [HttpGet("{id}")]
    public IActionResult GetTransactionById(string id)
    {
        return Ok(_repository.GetTransactionById(id));
    }

    [HttpGet]
    public IActionResult GetAllTransactions()
    {
        return Ok(_repository.GetAllTransactions());
    }
}