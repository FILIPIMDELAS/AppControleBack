using AppControle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppControle.Controllers;

[Route("api/work")]
[ApiController]

public class workController : ControllerBase
{
    private readonly AcessControlContext _context;

    public workController(AcessControlContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Work>>> GetTodoWork()
    {
        return await _context.Work.Select(x => ItemToDTO(x)).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Work>> GetWorkById(int id)
    {
        var work = await _context.Work.FindAsync(id);

        if (work == null)
        {
            return NotFound();
        }

        return ItemToDTO(work);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Work>> PutWork(int id, Work todoDTO)
    {
        var Work = await _context.Work.FindAsync(id);
        if (Work == null)
        {
            return NotFound();
        }

        Work.Id = id;
        Work.IdWork = todoDTO.IdWork;
        Work.Name = todoDTO.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!WorkExist(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Work>> PostWork(Work TodoDTO)
    {
        var Work = new Work
        {
            Id = TodoDTO.Id,
            IdWork = TodoDTO.IdWork,
            Name = TodoDTO.Name,
        };
        _context.Work.Add(Work);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoWork), new { Id = TodoDTO.Id }, ItemToDTO(Work));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWork(int id)
    {
        var Work = await _context.Work.FindAsync(id);
        if(Work == null)
        {
            return NotFound();
        }

        _context.Work.Remove(Work);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WorkExist(int id)
    {
        return _context.Work.Any(x => x.Id == id);
    }

    private static Work ItemToDTO(Work Work) =>
    new Work
    {
        Id = Work.Id,
        IdWork = Work.IdWork,
        Name = Work.Name,
    };
}