using BlazorEventEase.Client.Models;

namespace BlazorEventEase.Client.Services;

public sealed class InMemoryEventStore : IEventStore
{
    private readonly List<EventModel> events =
    [
        new()
        {
            Id = Guid.Parse("a471c057-e572-4e56-865d-24b068bb9191"),
            Name = "Conferencia de Tecnología",
            Date = new DateOnly(2026, 9, 15),
            Location = "Ciudad de México",
            Description = "Conferencia sobre tendencias y soluciones tecnológicas."
        },
        new()
        {
            Id = Guid.Parse("dc097d67-1776-443a-9803-3cabe076bf27"),
            Name = "Taller de Blazor",
            Date = new DateOnly(2026, 10, 5),
            Location = "Veracruz",
            Description = "Taller práctico de componentes, estado y navegación."
        }
    ];

    public IReadOnlyList<EventModel> GetAll()
    {
        return events
            .OrderBy(eventModel => eventModel.Date)
            .ThenBy(eventModel => eventModel.Name)
            .ToList();
    }

    public EventModel? GetById(Guid id)
    {
        return events.FirstOrDefault(eventModel => eventModel.Id == id);
    }

    public void Update(EventModel eventModel)
    {
        var index = events.FindIndex(item => item.Id == eventModel.Id);

        if (index < 0)
        {
            throw new KeyNotFoundException(
                $"No se encontró el evento con identificador {eventModel.Id}.");
        }

        events[index] = eventModel;
    }
}
