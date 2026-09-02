using BlazorEventEase.Client.Models;

namespace BlazorEventEase.Client.Services;

public interface IUserSessionTracker
{
    UserSession? CurrentSession { get; }

    bool IsInitialized { get; }

    bool IsActive { get; }

    event Action? SessionChanged;

    Task InitializeAsync();

    Task StartAsync(RegistrationModel registration);

    Task RegisterActivityAsync();

    Task EndAsync();
}