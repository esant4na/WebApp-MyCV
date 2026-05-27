using Microsoft.AspNetCore.Mvc;
using WebApp_MyCV.Models;
using WebApp_MyCV.Services;

namespace WebApp_MyCV.Controllers
{
    public class ContactController : Controller
    {
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Completa correctamente el formulario.";
                return Redirect("/#contact");
            }

            try
            {
                await _emailService.SendEmailAsync(model);

                TempData["Success"] = "Mensaje enviado correctamente.";
            }
            catch (Exception ex) {
            
                TempData["Error"] = "Error al enviar el mensaje.";
                Console.WriteLine("EMAIL ERROR: " + ex.Message);
            }
            TempData["Success"] = "Enviando mensaje...";
            return Redirect("/#contact");
        }
    }
}
