using Domain.Entities;
using MediatR;

namespace Application.Contact.Commands.SendContactMessage;

public class SendContactMessageHandler : IRequestHandler<SendContactMessageCommand, bool>
{
    private readonly IEmailSender _emailSender;

    public SendContactMessageHandler(IEmailSender emailSender) => _emailSender = emailSender;

    public async Task<bool> Handle(SendContactMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new ContactMessage
        {
            Name = request.Name,
            Email = request.Email,
            Message = request.Message
        };

        await _emailSender.SendAsync(message);
        return true;
    }
}