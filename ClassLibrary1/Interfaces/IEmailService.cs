using Sis_Inmobiliaria.Core.Application.Dtos.Email;

namespace Sis_Inmobiliaria.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto emailRequestDto);
    }
}
