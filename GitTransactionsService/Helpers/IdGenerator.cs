public static class IdGenerator
{
    public static string GenerateMerchantId()
    {
        return "MER-" + GenerateRandom10();
    }

    public static string GenerateTerminalId()
    {
        return "TRM-" + GenerateRandom10();
    }

    public static string GenerateTransactionRef()
    {
        return "TXN-" + GenerateRandom10();
    }

    private static string GenerateRandom10()
    {
        // Your exact logic
        return Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
    }
}