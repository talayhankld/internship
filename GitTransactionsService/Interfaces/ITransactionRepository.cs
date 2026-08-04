namespace GitTransactionsService.Interfaces;

public interface ITransactionRepository
{
    void AddTransaction(string id, string message);
    string GetTransactionById(string id);
    List<string> GetAllTransactions();
}
