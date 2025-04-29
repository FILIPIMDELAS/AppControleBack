using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppControle.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;

namespace AppControle.Controllers;

[Route("api/User")]
[ApiController]
public class userController : ControllerBase
{
    private readonly AcessControlContext _context;

    public userController(AcessControlContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Authorize(Policy = "Permission")]
    public async Task<ActionResult<IEnumerable<User>>> GetTodoUsuario()
    {
        return await _context.User
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUsuarioById(int id)
    {
        var Usuario = await _context.User.FindAsync(id);

        if (Usuario == null)
        {
            return NotFound();
        }

        return ItemToDTO(Usuario);
    }

    [HttpPost("recSenha")]
    public async Task<int> GetUsuarioByEmail([FromBody] string email)
    {
        var Usuario = await _context.User.FirstOrDefaultAsync(x => x.Email == email);

        if (Usuario == null)
        {

            return 404;
        }

        return Usuario.Id;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, User todoDTO)
    {
        var User = await _context.User.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        User.Id = id;
        User.Name = todoDTO.Name;
        User.Email = User.Email;
        User.Password = User.Password;
        User.Logradouro = todoDTO.Logradouro;
        User.Bairro = todoDTO.Bairro;
        User.Numero = todoDTO.Numero;
        User.Cidade = todoDTO.Cidade;
        User.Estado = todoDTO.Estado;
        User.Cep = todoDTO.Cep;
        User.Funcao = todoDTO.Funcao;

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
    public async Task<ActionResult<User>> PostUsuario(User todoDTO)
    {
        var emailUser = await _context.User.FirstOrDefaultAsync(x =>  x.Email == todoDTO.Email);
        if(emailUser  != null)
        {
            return Conflict(10001);
        }

        var User = new User
        {
            Id = todoDTO.Id,
            Name = todoDTO.Name,
            Email = todoDTO.Email,
            Password = PasswordHash.HashPassword(todoDTO.Password),
            Logradouro = todoDTO.Logradouro,
            Bairro = todoDTO.Bairro,
            Numero = todoDTO.Numero,
            Cidade = todoDTO.Cidade,
            Estado = todoDTO.Estado,
            Cep = todoDTO.Cep,
            Funcao = todoDTO.Funcao,
        };

        _context.User.Add(User);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoUsuario),
            new { id = User.Id },
            ItemToDTO(User));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var User = await _context.User.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        _context.User.Remove(User);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuarioExists(int id)
    {
        return _context.User.Any(e => e.Id == id);
    }

    private static User ItemToDTO(User User) =>
    new User
    {
        Id = User.Id,
        Name = User.Name,
        Email = User.Email,
        Password = User.Password,
        Logradouro = User.Logradouro,
        Bairro = User.Bairro,
        Numero = User.Numero,
        Cidade = User.Cidade,
        Estado = User.Estado,
        Cep = User.Cep,
        Funcao = User.Funcao,
    };
}
