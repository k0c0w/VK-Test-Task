using Contracts;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Users([FromQuery] int page = 1, [FromQuery] int limit = 16)
    {
        if (page < 1) page = 1;
        if (limit < 1) limit = 16;
        var users = await _service.GetManyUserInfosAsync(offset: (page - 1) * limit, limit);
        return new JsonResult(users.Select(x => new UserVM
        {
            Id = x.Id,
            Login = x.Login,
            CreatedDate = DateOnly.FromDateTime(x.CreatedDate),
            Group = new UserGroupVM { Code = x.Group.Code.ToString(), Description = x.Group.Description },
            State = new UserStateVM { Code = x.State.Code.ToString(), Description = x.State.Description }
        }));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserVM user)
    {
        try
        {
            await _service.CreateUserAsync(new CreateUserDto
                { Login = user.Login, Password = user.Password, GroupId = user.GroupId });
            return new CreatedResult("/users", null);
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { error = "Admin creation denied." });
        }
        catch (UserAlreadyExistsException)
        {
            return BadRequest(new {error="Can not create user."});
        }
        catch (ObjectNotFoundException)
        {
            return BadRequest(new { error = "No such group." });
        }
    }

    [HttpPut("block/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await _service.DeleteUserByIdAsync(id);
        return NoContent();
    }
}