namespace PremierBankTesting.Contracts
{
    public record PremierBankTestingResponse
    (
        Guid Id,
        decimal amount,
        string comment,
        DateTime timestamp,
        string userEmail,
        bool isProcessed = false

    );
    public record PremierBankTestingRequest(
      decimal amount,
       string comment,
       DateTime timestamp,
       string userEmail,
        bool isProcessed = false
       );
}
