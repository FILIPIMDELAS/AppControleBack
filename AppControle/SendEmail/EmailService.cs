using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static ExportEXCELController;
using AppControle.Controllers;

public class EmailService
{
    private readonly ExportEXCELController _exportService;
    private readonly string _smtpHost = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _emailSender = "filipecristovam2020@gmail.com";
    private readonly string _emailSenderPassword = "xaoc twsi ozpt hvjg";

    public EmailService(ExportEXCELController exportService)
    {
        _exportService = exportService;
    }

    public async Task SendVerificationEmailAsync(string recipientEmail, string verificationCode)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSender),
                Subject = "Código de Verificação",
                Body = $"Seu código de verificação é: {verificationCode}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(recipientEmail);

            using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSender, _emailSenderPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }

            Console.WriteLine("E-mail enviado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
        }
    }

    public async Task SendEmailExcelAsync (string email, string titulo, string conteudo, List<DadosReformContract> dados)
    {
        try
        {
            var excelBytes = await _exportService.ExportReformToExcel(dados);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSender),
                Subject = titulo,
                Body = conteudo,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            var memoryStream = new MemoryStream(excelBytes);

            try
            {
                memoryStream.Position = 0;

                var attachment = new Attachment(memoryStream, "Reforma.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                mailMessage.Attachments.Add(attachment);

                Console.WriteLine("Anexo adicionado ao e-mail com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar anexo: {ex.Message}");
                return;
            }

            using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSender, _emailSenderPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
            }
        }
    }
}
