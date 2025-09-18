using EventForge.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

public class IndexModel(IHttpClientFactory httpFactory) : PageModel
{
    public List<EventDto> Events { get; private set; } = [];
    [BindProperty]
    public EventDto Form
    {
        get; set;
    } =
        new(null, "", EventCategory.Other, "", DateOnly.FromDateTime(DateTime.Today), new TimeOnly(12, 0), "", null, null);

    public IEnumerable<SelectListItem> Categories => Enum.GetValues<EventCategory>()
        .Select(c => new SelectListItem(c.ToString(), c.ToString()));

    public async Task OnGetAsync()
    {
        var client = httpFactory.CreateClient("EventsApi");

        Events = await client.GetFromJsonAsync<List<EventDto>>("events") ?? [];
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        var client = httpFactory.CreateClient("EventsApi");

        var createReq = new
        {
            Event = new
            {
                Name = Form.Name,
                Category = Form.Category,
                Place = Form.Place,
                Date = Form.Date,
                Time = Form.Time,
                Description = Form.Description,
                AdditionalInfo = Form.AdditionalInfo,
                ImageUrl = Form.ImageUrl
            }
        };

        var resp = await client.PostAsJsonAsync("/events", createReq);
        if (!resp.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, $"Create failed ({(int)resp.StatusCode})");
        }

        await OnGetAsync();
        Form = Form with
        {
            Name = "",
            Place = "",
            Description = "",
            AdditionalInfo = "",
            ImageUrl = ""
        };

        return Page();
    }
}
