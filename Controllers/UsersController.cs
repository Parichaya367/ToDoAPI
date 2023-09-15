using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace ToDoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var db = new ToDoDbContext();

        var users = from a in db.User select a;
        if (!users.Any()) return NoContent();

        return Ok(users);
    }

    [Route("{id}")]
    [HttpGet]
    public IActionResult Get(string id)
    {
        var db = new ToDoDbContext();

        var user = db.User.Find(id);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public IActionResult Post([FromBody] DTOs.User data)
    {
        var db = new ToDoDbContext();

        var user = new Models.User();
        if (data.Id == null) return BadRequest("Id can't be null");
        user.Id = data.Id;
        data.Salt = "KFf+BFt6IQT6YxMbNLJU8A==";
        user.Salt = data.Salt;
        if (data.Password == null) return BadRequest("Password can't be null");
        string hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: data.Password,
                salt: Convert.FromBase64String(user.Salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            )
        );
        user.Password = hash;

        db.User.Add(user);
        db.SaveChanges();
        return Ok(user);
    }

    [Route("{id}")]
    [HttpPut]
    public IActionResult Put(string id, [FromBody] DTOs.User data)
    {
        var db = new ToDoDbContext();

        var user = db.User.Find(id);
        if (user == null) return NotFound();
        data.Salt = "KFf+BFt6IQT6YxMbNLJU8A==";
        user.Salt = data.Salt;
        if (data.Password == null) return BadRequest("Password can't be null");
        string hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: data.Password,
                salt: Convert.FromBase64String(user.Salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            )
        );
        user.Password = hash;

        db.SaveChanges();
        return Ok(user);
    }

    [Route("{id}")]
    [HttpDelete]
    public IActionResult Delete(string id)
    {
        var db = new ToDoDbContext();

        var user = db.User.Find(id);
        if (user == null) return NotFound();
        db.User.Remove(user);
        db.SaveChanges();

        return Ok();
    }
}
