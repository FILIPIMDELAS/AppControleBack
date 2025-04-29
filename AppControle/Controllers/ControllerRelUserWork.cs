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
[Route("api/relUserWork")]
public class relUserWork : ControllerBase
{
    private readonly AcessControlContext _context;

    public relUserWork(AcessControlContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RelUserWork>>> GetUserWork()
    {
        return await _context.RelUserWork.Select(x => ItemToDTO(x)).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<RelUserWork>> GetTodoRel(RelUserWork todoDTO)
    {
        var UserWork = new RelUserWork
        {
            Id = todoDTO.Id,
            UserId = todoDTO.UserId,
            User = todoDTO.User,
            WorkId = todoDTO.WorkId,
            Work = todoDTO.Work,
        };

        _context.RelUserWork.Add(UserWork);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoRel),
            new { id = UserWork.Id },
            ItemToDTO(UserWork));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRel(int id)
    {
        var UserWork = await _context.RelUserWork.FindAsync(id);
        if (UserWork == null)
        {
            return NotFound();
        }

        _context.RelUserWork.Remove(UserWork);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static RelUserWork ItemToDTO(RelUserWork RelUserWork) =>
       new RelUserWork
       {
           Id = RelUserWork.Id,
           UserId = RelUserWork.UserId,
           User = RelUserWork.User,
           WorkId = RelUserWork.WorkId,
           Work = RelUserWork.Work,
       };
}