// Controllers/InstallmentController.cs
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using InstallmentLoan.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class InstallmentController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public InstallmentController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("DisbursementService");
    }

    [HttpGet("ping")]
    public IActionResult Ping() => Ok("InstallmentService is running.");


    [HttpPost("disburse")]
    public async Task<IActionResult> DisburseLoan([FromBody] DisbursementRequest request)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");
            

        var response = await _httpClient.PostAsync("/api/disbursement", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Disbursement failed.");
        }

        var resultJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DisbursementResponse>(resultJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return Ok(result);
    }
}
