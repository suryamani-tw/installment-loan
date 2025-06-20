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

    private readonly IPactBuilderV3 _pact;

    public InstallmentConsumerPactTests()
    {
        var config = new PactConfig
        {
            PactDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..","..","..", "pacts", Type, Provider),
            LogLevel = PactLogLevel.Debug
        };

        _pact = Pact.V3(Consumer, Provider, config).WithHttpInteractions();
    }

    [Fact]
    public async Task PostDisbursement_WhenValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        const string interactionDescription = "A valid disbursement request";
        const string endpoint = "/api/disbursement";
        
        _pact
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
        await _pact.VerifyAsync(async ctx =>
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
