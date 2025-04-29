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
[Route("api/permission")]
public class userPermissionController : ControllerBase
{
    private readonly AcessControlContext _context;

    public userPermissionController (AcessControlContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserPermission>>> GetPermission()
    {
        return await _context.UserPermission.Select(x => ItemToDTO(x)).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserPermission>> GetUserPermission(int id)
    {
        var Usuario = await _context.UserPermission.FindAsync(id);

        if (Usuario == null)
        {
            return NotFound();
        }

        return ItemToDTO(Usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> GetTodoUsuario(int id, UserPermission todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var User = await _context.UserPermission.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        User.Id = todoDTO.Id;
        User.NamePermission = todoDTO.NamePermission;
        User.DescriptionPermission = todoDTO.DescriptionPermission;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UsuarioExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<UserPermission>> GetTodoUsuario(UserPermission todoDTO)
    {
        var User = new UserPermission
        {
            Id = todoDTO.Id,
            NamePermission = todoDTO.NamePermission,
            DescriptionPermission = todoDTO.DescriptionPermission,
        };

        _context.UserPermission.Add(User);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoUsuario),
            new { id = User.Id },
            ItemToDTO(User));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoUsuario(int id)
    {
        var User = await _context.UserPermission.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        _context.UserPermission.Remove(User);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuarioExists(int id)
    {
        return _context.UserPermission.Any(e => e.Id == id);
    }

    private static UserPermission ItemToDTO(UserPermission userPermission) =>
       new UserPermission
       {
           Id = userPermission.Id,
           NamePermission = userPermission.NamePermission,
           DescriptionPermission = userPermission.DescriptionPermission,
       };
}
