using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using PactNet;
using PactNet.Matchers;
using Xunit;

public class InstallmentConsumerPactTests
{
    private const string Consumer = "installment";
    private const string Provider = "disbursement";
    private const string Type = "api";

    private readonly IPactBuilderV4 _httpPact;

    public InstallmentConsumerPactTests()
    {
        var repoRoot = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var config = new PactConfig
        {
            PactDir = Path.Combine(repoRoot, "pacts", Type),
            LogLevel = PactLogLevel.Debug
        };

        _httpPact = Pact.V4(Consumer, Provider, config).WithHttpInteractions();
    }

    [Fact]
    public async Task Generate_Pact_Initiate_Disbursement_Request()
    {
        // Arrange
        const string interactionDescription = "A valid initiate disbursement request";
        const string endpoint = "/api/disbursement";
        
        _httpPact
            .UponReceiving(interactionDescription)
            .WithRequest(HttpMethod.Post, endpoint)
            .WithHeader("Content-Type", "application/json")
            .WithJsonBody(new
            {
                LoanId = Match.Type("12345"),
                Amount = Match.Type(1000.00M)
            })
            .WillRespond()
            .WithStatus(200)
            .WithHeader("Content-Type", "application/json")
            .WithJsonBody(new
            {
                Message = Match.Type("Success"),
                DisbursementId = Match.Type("abc-123")
            });

        // Act & Assert
        await _httpPact.VerifyAsync(async ctx =>
        {
            using var httpClient = new HttpClient { BaseAddress = ctx.MockServerUri };
            var request = new
            {
                LoanId = "12345",
                Amount = 1000.00M
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            Assert.Equal("Success", doc.RootElement.GetProperty("Message").GetString());
            Assert.Equal("abc-123", doc.RootElement.GetProperty("DisbursementId").GetString());
        });
    }

}
