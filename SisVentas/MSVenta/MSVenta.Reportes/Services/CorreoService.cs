using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MSVenta.Reportes.Services
{
    public class DestinatarioCorreo
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }

    public class CorreoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CorreoService> _logger;

        public CorreoService(IConfiguration configuration, ILogger<CorreoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task EnviarReporteAsync(
            List<DestinatarioCorreo> destinatarios,
            string asunto,
            string cuerpoHtml,
            byte[] adjuntoPdf = null,
            string nombreAdjunto = null,
            string remitenteNombre = null,
            string remitenteEmail = null)
        {
            var host = _configuration["Correo:Host"];
            var puerto = int.Parse(_configuration["Correo:Puerto"]);

            var nombreFinal = !string.IsNullOrWhiteSpace(remitenteNombre)
                ? remitenteNombre
                : _configuration["Correo:RemitenteName"];

            var emailFinal = !string.IsNullOrWhiteSpace(remitenteEmail)
                ? remitenteEmail
                : _configuration["Correo:RemitenteEmail"];

            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress(nombreFinal, emailFinal));

            foreach (var d in destinatarios)
            {
                mensaje.To.Add(new MailboxAddress(d.Nombre, d.Correo));
            }

            mensaje.Subject = asunto;

            var builder = new BodyBuilder();
            builder.HtmlBody = cuerpoHtml;

            if (adjuntoPdf != null && nombreAdjunto != null)
            {
                builder.Attachments.Add(
                    nombreAdjunto,
                    adjuntoPdf,
                    new ContentType("application", "pdf"));
            }

            mensaje.Body = builder.ToMessageBody();

            // ─── Envío por SMTP ───────────────────────────────
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(host, puerto, SecureSocketOptions.None);
                await smtp.SendAsync(mensaje);
                await smtp.DisconnectAsync(true);
            }

            // ─── Guardar copia en "Enviados" del remitente ────
            // Si esto falla, no interrumpe el flujo principal: el correo
            // YA se envió correctamente por SMTP, esto es un extra.
            // El error queda registrado en el log para diagnóstico,
            // pero el usuario nunca ve un fallo por esta parte.
            try
            {
                await GuardarCopiaEnviadaAsync(mensaje, emailFinal);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "No se pudo guardar la copia en Enviados para {RemitenteEmail}",
                    emailFinal);
            }
        }

        private async Task GuardarCopiaEnviadaAsync(MimeMessage mensaje, string remitenteEmail)
        {
            if (string.IsNullOrWhiteSpace(remitenteEmail))
                return;

            // El username de Dovecot es la parte antes del @
            // (ej: "lili@tecnoweb.net" -> "lili")
            var username = remitenteEmail.Split('@')[0];

            var imapHost = _configuration["Correo:ImapHost"] ?? _configuration["Correo:Host"];
            var imapPuerto = int.Parse(_configuration["Correo:ImapPuerto"] ?? "143");
            var masterUser = _configuration["Correo:MasterUser"];
            var masterPassword = _configuration["Correo:MasterPassword"];

            // Formato de master user en Dovecot: usuario*usuariomaestro
            var loginCompuesto = $"{username}*{masterUser}";

            using var imap = new ImapClient();
            await imap.ConnectAsync(imapHost, imapPuerto, SecureSocketOptions.None);
            await imap.AuthenticateAsync(loginCompuesto, masterPassword);

            var carpetaEnviados = await imap.GetFolderAsync("Sent");
            await carpetaEnviados.OpenAsync(FolderAccess.ReadWrite);
            await carpetaEnviados.AppendAsync(mensaje, MessageFlags.Seen);

            await imap.DisconnectAsync(true);
        }
    }
}