using MediatR;

namespace Application.Contact.Commands.SendContactMessage;

public record SendContactMessageCommand(string Name, string Email, string Message) : IRequest<bool>;