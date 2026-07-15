using System.ComponentModel.DataAnnotations;
using Application.Contact.Commands.SendContactMessage;
using Application.Experience.Queries.GetAllExperience;
using Application.Home.Queries.GetHomeContent;
using Application.Projects.Queries.GetAllProjects;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public HomeContent Content { get; set; } = null!;
    public List<ExperienceEntry> Experience { get; set; } = [];
    public List<Project> Projects { get; set; } = [];

    [BindProperty]
    public ContactFormInput Input { get; set; } = new();

    public bool MessageSent { get; set; }

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task OnGetAsync() => await LoadContentAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadContentAsync();
            return Page();
        }

        await _mediator.Send(new SendContactMessageCommand(Input.Name, Input.Email, Input.Message));
        MessageSent = true;

        await LoadContentAsync();
        return Page();
    }

    private async Task LoadContentAsync()
    {
        Content = await _mediator.Send(new GetHomeContentQuery("en"));
        Experience = await _mediator.Send(new GetAllExperienceQuery("en"));
        Projects = await _mediator.Send(new GetAllProjectsQuery("en"));
    }

    public class ContactFormInput
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, StringLength(2000)]
        public string Message { get; set; } = "";
    }
}