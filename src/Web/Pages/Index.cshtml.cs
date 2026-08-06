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
    private readonly ILogger<IndexModel> _logger;

    public HomeContent Content { get; set; } = null!;
    public List<ExperienceEntry> Experience { get; set; } = [];
    public List<Project> Projects { get; set; } = [];
    public string Culture { get; set; } = "en";

    [BindProperty]
    public ContactFormInput Input { get; set; } = new();

    public bool MessageSent { get; set; }
    public bool MessageFailed { get; set; }

    public IndexModel(IMediator mediator, ILogger<IndexModel> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task OnGetAsync(string? lang)
    {
        if (!string.IsNullOrEmpty(lang) && (lang == "en" || lang == "fa"))
        {
            Response.Cookies.Append("culture", lang, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            Culture = lang;
        }
        else
        {
            Culture = Request.Cookies["culture"] ?? "en";
        }

        ViewData["Culture"] = Culture;
        await LoadContentAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Culture = Request.Cookies["culture"] ?? "en";
        ViewData["Culture"] = Culture;

        if (!ModelState.IsValid)
        {
            await LoadContentAsync();
            return Page();
        }

        try
        {
            await _mediator.Send(new SendContactMessageCommand(Input.Name, Input.Email, Input.Message));
            MessageSent = true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "The portfolio contact message could not be sent.");
            MessageFailed = true;
        }

        await LoadContentAsync();
        return Page();
    }

    private async Task LoadContentAsync()
    {
        Content = await _mediator.Send(new GetHomeContentQuery(Culture));
        Experience = await _mediator.Send(new GetAllExperienceQuery(Culture));
        Projects = await _mediator.Send(new GetAllProjectsQuery(Culture));
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
