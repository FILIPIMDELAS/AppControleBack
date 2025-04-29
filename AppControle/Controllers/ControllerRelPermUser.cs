using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppControle.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace AppControle.Controllers;

[ApiController]
[Route("api/relPermUser")]
public class relPermUser : ControllerBase
{
    private readonly AcessControlContext _context;

    public relPermUser(AcessControlContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RelPermUser>>> GetPermission()
    {
        return await _context.RelPermUser.Select(x => ItemToDTO(x)).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RelPermUser>> GetUserPermission(int id)
    {
        var Usuario = await _context.RelPermUser.FindAsync(id);

        if (Usuario == null)
        {
            return NotFound();
        }

        return ItemToDTO(Usuario);
    }

    [HttpPost]
    public async Task<ActionResult<RelPermUser>> GetTodoUsuario(RelPermUser todoDTO)
    {
        var User = new RelPermUser
        {
            Id = todoDTO.Id,
            UserId = todoDTO.UserId,
            User = todoDTO.User,
            UserPermissionId = todoDTO.UserPermissionId,
            UserPermission = todoDTO.UserPermission,
        };

        _context.RelPermUser.Add(User);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoUsuario),
            new { id = User.Id },
            ItemToDTO(User));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoUsuario(int id)
    {
        var User = await _context.RelPermUser.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        _context.RelPermUser.Remove(User);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static RelPermUser ItemToDTO(RelPermUser relPermissionGroup) =>
       new RelPermUser
       {
           Id = relPermissionGroup.Id,
           UserId = relPermissionGroup.UserId,
           User = relPermissionGroup.User,
           UserPermissionId = relPermissionGroup.UserPermissionId,
           UserPermission = relPermissionGroup.UserPermission,
       };
}