namespace GitTransactionsService.Services;

using GitTransactionsService.Interfaces;

public class TransactionRepository
{
    private List<string> _transactions = new List<string>();
    public void AddTransaction(string id, string message)
    {
        _transactions.Add($"{id}: {message}");
    }
    public string GetTransactionById(string id)
    {
        return _transactions.FirstOrDefault(t => t.StartsWith(id)) ?? "Not found";
    }
    public List<string> GetAllTransactions()
    {
        return _transactions;
    }
}
    