using PactNet;
using PactNet.Matchers;
using Xunit;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;



public class InstallmentConsumerMessagePactTests
{
    private readonly IMessagePactBuilderV4 _pact;

    private const string Consumer = "installment";
    private const string Provider = "disbursement";
    private const string Type = "message";


    public InstallmentConsumerMessagePactTests()
    {
        var repoRoot = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        _pact = Pact.V4(Consumer, Provider, new PactConfig
        {
            PactDir = Path.Combine(repoRoot, "pacts", Type),
            LogLevel = PactLogLevel.Debug
        })
        .WithMessageInteractions();
    }

    [Fact]
    public async Task Generate_Pact_Disbursement_Status_Message()
    {
        // Arrange
        var expectedMessage = new
        {
            LoanId = Match.Type("IL1234"),
            Amount = Match.Type(1000.00M),
            Status = Match.Type("Success"),
            DisbursementId = Match.Regex("abc-123", @"^[a-z]{3}-\d{3}$")
        };

        // Act & Assert (contract setup and verification)
        await _pact
            .ExpectsToReceive("A disbursement status message")
            .WithJsonContent(expectedMessage)
            .VerifyAsync<JsonElement>(message =>
            {
                Assert.Equal("IL1234", message.GetProperty("LoanId").GetString());
                Assert.Equal(1000.00M, message.GetProperty("Amount").GetDecimal());
                Assert.Equal("Success", message.GetProperty("Status").GetString()); 
                Assert.Equal("abc-123", message.GetProperty("DisbursementId").GetString());
                return Task.CompletedTask;
            });
    }
}
