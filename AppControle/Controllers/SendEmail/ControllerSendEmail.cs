using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ExportEXCELController;


namespace YourNamespace.Controllers;

[Route("api/SendEmail")]
[ApiController]
public class VerificationController : ControllerBase
{
    private readonly VerificationService _verificationService;
    private readonly EmailService _emailService;
    private readonly IMemoryCache _memoryCache;

    public VerificationController(VerificationService verificationService, EmailService emailService, IMemoryCache memoryCache)
    {
        _verificationService = verificationService;
        _emailService = emailService;
        _memoryCache = memoryCache;
    }

    [HttpPost]
    public async Task<IActionResult> SendVerificationEmail([FromBody] string recipientEmail)
    {
        try
        {
            string verificationCode = _verificationService.GenerateVerificationCode();

            _memoryCache.Set(recipientEmail, verificationCode, TimeSpan.FromMinutes(5));

            await _emailService.SendVerificationEmailAsync(recipientEmail, verificationCode);

            return Ok(new { message = "E-mail de verificação enviado com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro ao enviar o e-mail: {ex.Message}" });
        }
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyCode([FromBody] VerificationRequest verificationRequest)
    {
        Console.WriteLine("ClientCode: " + verificationRequest.CodeFromClient);
        Console.WriteLine("RecipientEmail: " + verificationRequest.RecipientEmail);

        if (_memoryCache.TryGetValue(verificationRequest.RecipientEmail, out string serverCode))
        {
            Console.WriteLine("ServerCode from cache: " + serverCode);
            if (serverCode == verificationRequest.CodeFromClient)
            {
                return Ok(new { message = "Código de verificação válido!" });
            }
        }

        return StatusCode(500, new { message = "Código de verificação inválido ou expirado."});
    }

    public class VerificationRequest
    {
        public string CodeFromClient { get; set; }
        public string RecipientEmail { get; set; }
    }

    [HttpPost("exportReform")]
    public async Task<IActionResult> SendExcelnEmail([FromBody] EmailRequest request)
    {
        try
        {
            if (request.dados == null || !request.dados.Any())
            {
                return BadRequest(new { message = "Dados inválidos ou vazios." });
            }

            await _emailService.SendEmailExcelAsync(request.email, request.titulo, request.conteudo, request.dados);

            return Ok(new { message = "E-mail de verificação enviado com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro ao enviar o e-mail: {ex.Message}" });
        }
    }

    public class EmailRequest
    {
        public string email { get; set; }
        public string titulo { get; set; }
        public string conteudo { get; set; }
        public List<DadosReformContract> dados { get; set; }
    }
}
