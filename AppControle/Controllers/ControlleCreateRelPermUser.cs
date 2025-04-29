using AppControle.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppControle.Controllers;

[ApiController]
[Route("api/createRelPermUser")]
public class CreateRelPermUserController : ControllerBase
{
    private readonly AcessControlContext _context;

    public CreateRelPermUserController(AcessControlContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRelationship([FromBody] CreateRelPermUser relDto)
    {
        var Permission = await _context.UserPermission.FindAsync(relDto.UserPermissionId);
        var user = await _context.User.FindAsync(relDto.UserId);

        var rel = new RelPermUser
        {
            Id = relDto.Id,
            UserId = relDto.UserId,
            User = user,
            UserPermissionId = relDto.UserPermissionId,
            UserPermission = Permission,
        };

        _context.RelPermUser.Add(rel);
        await _context.SaveChangesAsync();

        return Ok(rel);
    }
}