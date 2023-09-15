using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivitiesController : ControllerBase
{

    private readonly ILogger<ActivitiesController> _logger;

    public ActivitiesController(ILogger<ActivitiesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public IActionResult Get()
    {
        var db = new ToDoDbContext();
        var activities = from a in db.Activity where a.UserId.Equals(User.Identity.Name) select a;
        if (!activities.Any()) return NoContent();

        return Ok(activities);
    }

    [Route("{id}")]
    [HttpGet]
    [Authorize(Roles = "user")]
    public IActionResult Get(uint id)
    {
        var db = new ToDoDbContext();

        var activity = db.Activity.Find(id);
        if (activity == null) return NotFound();

        return Ok(activity);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IActionResult Post([FromBody] DTOs.Activity data)
    {
        var db = new ToDoDbContext();

        var activity = new Models.Activity
        {
            UserId = User.Identity.Name,
            Name = data.Name,
            When = data.When
        };

        db.Activity.Add(activity);
        db.SaveChanges();
        return Ok();
    }

    [Route("{id}")]
    [HttpPut]
    [Authorize(Roles = "user")]
    public IActionResult Put(uint id, [FromBody] DTOs.Activity data)
    {
        var db = new ToDoDbContext();

        var activity = db.Activity.Find(id);
        if (activity == null) return NotFound();
        activity.UserId = User.Identity.Name;
        if (data.Name == null) return BadRequest("Name can't be null");
        activity.Name = data.Name;
        activity.When = data.When;

        db.SaveChanges();
        return Ok();
    }

    [Route("{id}")]
    [HttpDelete]
    [Authorize(Roles = "user")]
    public IActionResult Delete(uint id)
    {
        var db = new ToDoDbContext();

        var activity = db.Activity.Find(id);
        if (activity == null) return NotFound();
        db.Activity.Remove(activity);
        db.SaveChanges();

        return Ok();
    }
}
