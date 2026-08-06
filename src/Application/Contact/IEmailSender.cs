using Domain.Entities;

namespace Application.Contact;

public interface IEmailSender
{
    Task SendAsync(ContactMessage message);
}