namespace EventForge.API.Dtos.Validators;

public sealed class EventDtoValidator : AbstractValidator<EventDto>
{
    public EventDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters long.");

        RuleFor(x => x.Category)
            .Must(c => Enum.IsDefined(typeof(EventCategory), c))
            .WithMessage("Category value is invalid.");

        RuleFor(x => x.Place)
            .NotEmpty().WithMessage("Place is required.")
            .MinimumLength(3).WithMessage("Place must be at least 3 characters long.")
            .MaximumLength(200).WithMessage("Place must be at most 200 characters long.");

        RuleFor(x => x.Date)
            .Must(d => d != default).WithMessage("Date is required.");

        RuleFor(x => x.Time)
            .Must(t => t != default).WithMessage("Time is required.");

        RuleFor(x => new { x.Date, x.Time })
            .Must(dt =>
            {
                var start = dt.Date.ToDateTime(dt.Time);
                return start >= DateTime.Now;
            })
            .WithMessage("Event start time must not be in the past.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must be at most 1000 characters long.");

        RuleFor(x => x.AdditionalInfo)
            .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.AdditionalInfo))
            .WithMessage("AdditionalInfo must be at most 500 characters long.");

        RuleFor(x => x.ImageUrl)
            .Must(BeValidHttpUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl must be an absolute http/https URL.")
            .Must(u => u!.Length <= 2048)
            .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl is too long (max 2048).");
    }

    private static bool BeValidHttpUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;
        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
            return false;
        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }
}
