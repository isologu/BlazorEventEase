namespace BlazorEventEase.Client.Models;

public sealed class UserSession
{
    public Guid SessionId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTimeOffset StartedAtUtc { get; set; }

    public DateTimeOffset LastActivityAtUtc { get; set; }

    public bool IsActive { get; set; }
}