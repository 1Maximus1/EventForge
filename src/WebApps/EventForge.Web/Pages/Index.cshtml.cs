using EventForge.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public IndexModel(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

    public List<EventFullDto> Events { get; private set; } = [];

    [BindProperty]
    public EventFullDto Form
    {
        get; set;
    } = new(
        Guid.Empty, "", EventCategory.Other, "",
        DateOnly.FromDateTime(DateTime.Today), new TimeOnly(12, 0),
        "", null, null
    );

    public IEnumerable<SelectListItem> Categories =>
        Enum.GetValues<EventCategory>()
            .Select(c => new SelectListItem(c.ToString(), ((int)c).ToString()));

    public async Task OnGetAsync() => await LoadEventsAsync();

    public async Task<IActionResult> OnPostUpsertAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadEventsAsync();
            return Page();
        }

        var client = _httpFactory.CreateClient("EventsApi");
        var dto = new EventDto(Form.Name, Form.Category, Form.Place, Form.Date, Form.Time,
                               Form.Description, Form.AdditionalInfo, Form.ImageUrl);

        HttpResponseMessage resp = Form.Id == Guid.Empty
            ? await client.PostAsJsonAsync("events", new CreateEventRequest(dto), JsonOpts)
            : await client.PutAsJsonAsync($"events/{Form.Id}", new UpdateEventRequest(dto), JsonOpts);

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"{(Form.Id == Guid.Empty ? "Create" : "Update")} failed ({(int)resp.StatusCode}): {body}");
            await LoadEventsAsync();
            return Page();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var client = _httpFactory.CreateClient("EventsApi");
        await client.DeleteAsync($"events/{id}");
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync(Guid id)
    {
        await LoadEventsAsync();

        var ev = Events.FirstOrDefault(x => x.Id == id);
        if (ev is null)
        {
            ModelState.AddModelError("", "Event not found.");
            return Page();
        }

        ModelState.Clear();
        Form = ev;
        return Page();
    }

    public IActionResult OnPostCancel()
    {
        ModelState.Clear();
        Form = new EventFullDto(
            Guid.Empty, "", EventCategory.Other, "",
            DateOnly.FromDateTime(DateTime.Today), new TimeOnly(12, 0),
            "", null, null
        );
        return RedirectToPage();
    }

    private async Task LoadEventsAsync()
    {
        var client = _httpFactory.CreateClient("EventsApi");
        var response = await client.GetFromJsonAsync<GetEventsResponse>("events", JsonOpts);
        Events = response?.Events?.ToList() ?? [];
    }
}
