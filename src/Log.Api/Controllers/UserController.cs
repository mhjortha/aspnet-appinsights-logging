using Log.Api.Contracts;
using Log.Api.Models;
using Log.Api.Persistence;
using Log.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Log.Api.Controllers;

[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILoggerAdapter<UserController> _logger;

    public UserController(IUserRepository userRepository, ILoggerAdapter<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    
    [HttpGet("/health")]
    public async Task<IActionResult> GetHealthAsync()
    {
        return Ok("Healthy");
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            _logger.LogWarning("Failed retrieving user with id: {UserId}, was not found", id);
            return NotFound();
        }
        _logger.LogInformation("Successfully retrieved user {UserId}", user.Id);
        
        return Ok(user);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        _logger.LogInformation("Successfully retrieved {Count} users", users.Count);
        return Ok(users);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody]CreateUserRequest request)
    {
        var user = new User
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Age = request.Age
        };
        await _userRepository.AddAsync(user);
        _logger.LogInformation("Successfully created user: {UserId}", user.Id);
        return Created("", user);
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            _logger.LogWarning("Failed deleting user with id: {UserId}, was not found", id);
            return NotFound();
        }
        
        await _userRepository.DeleteAsync(user);
        _logger.LogInformation("Successfully deleted user {UserId}", user.Id);
        return NoContent();
    }
}