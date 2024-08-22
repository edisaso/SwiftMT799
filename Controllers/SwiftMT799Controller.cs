using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SwiftMT799Controller : ControllerBase
{
    private readonly SwiftMT799Parser _parser;
    private readonly DatabaseService _dbService;
    private readonly ILogger<SwiftMT799Controller> _logger;

    public SwiftMT799Controller(SwiftMT799Parser parser, DatabaseService dbService, ILogger<SwiftMT799Controller> logger)
    {
        _parser = parser;
        _dbService = dbService;
        _logger = logger;
    }

    /// <summary>
    /// Processes a Swift MT799 message
    /// </summary>
    /// <returns>A confirmation that the message was processed</returns>
    /// <response code="200">Returns when the message is successfully processed</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProcessMessage()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            var messageContent = await reader.ReadToEndAsync();

            var message = _parser.Parse(messageContent);
            await _dbService.SaveMessageAsync(message);

            _logger.LogInformation("Message processed successfully");
            return Ok("Message processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            return StatusCode(500, "An error occurred while processing the message");
        }
    }
}
