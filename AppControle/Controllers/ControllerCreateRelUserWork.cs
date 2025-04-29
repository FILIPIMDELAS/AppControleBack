using AppControle.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppControle.Controllers;

[ApiController]
[Route("api/createRelUserWork")]
public class CreateRelUserWorkController : ControllerBase
{
    private readonly AcessControlContext _context;

    public CreateRelUserWorkController(AcessControlContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRelUserWork([FromBody] CreateRelUserWork relDto)
    {
        var work = await _context.Work.FindAsync(relDto.WorkId);
        var user = await _context.User.FindAsync(relDto.UserId);

        var rel = new RelUserWork
        {
            Id = relDto.Id,
            UserId = relDto.UserId,
            User = user,
            WorkId = relDto.WorkId,
            Work = work,
        };

        _context.RelUserWork.Add(rel);
        await _context.SaveChangesAsync();

        return Ok(rel);
    }
}