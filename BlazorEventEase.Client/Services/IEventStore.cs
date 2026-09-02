using BlazorEventEase.Client.Models;

namespace BlazorEventEase.Client.Services;

public interface IEventStore
{
    IReadOnlyList<EventModel> GetAll();

    EventModel? GetById(Guid id);

    void Update(EventModel eventModel);
}