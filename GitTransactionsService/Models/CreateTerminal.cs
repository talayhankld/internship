using System.ComponentModel;
namespace GitTransactionsService.Models;

public class CreateTerminal
{
    [DefaultValue("1")]
    public string TerminalNo{ get; set; } = string.Empty;

    [DefaultValue("Pos 1")]
    public string TerminalName { get; set; } = string.Empty;

    [DefaultValue("TRY")]
    public string Currency { get; set; } = string.Empty;
}