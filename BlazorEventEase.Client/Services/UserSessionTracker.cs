using System.Text.Json;
using BlazorEventEase.Client.Models;
using Microsoft.JSInterop;

namespace BlazorEventEase.Client.Services;

public sealed class UserSessionTracker : IUserSessionTracker, IAsyncDisposable
{
    private const string StorageKey = "eventease.user-session";

    private readonly IJSRuntime jsRuntime;
    private readonly JsonSerializerOptions jsonOptions;

    private IJSObjectReference? module;
    private bool isDisposed;

    public UserSessionTracker(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;

        jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public UserSession? CurrentSession { get; private set; }

    public bool IsInitialized { get; private set; }

    public bool IsActive =>
        CurrentSession is
        {
            IsActive: true
        };

    public event Action? SessionChanged;

    public async Task InitializeAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        try
        {
            var storageModule = await GetModuleAsync();

            var json = await storageModule.InvokeAsync<string?>(
                "getItem",
                StorageKey);

            if (!string.IsNullOrWhiteSpace(json))
            {
                CurrentSession =
                    JsonSerializer.Deserialize<UserSession>(
                        json,
                        jsonOptions);
            }

            IsInitialized = true;
            NotifySessionChanged();
        }
        catch (InvalidOperationException)
        {
            // JavaScript todavía no está disponible durante
            // la representación previa. Se intentará nuevamente
            // después del primer render interactivo.
        }
        catch (JSDisconnectedException)
        {
            // El circuito se desconectó antes de terminar la operación.
        }
    }

    public async Task StartAsync(RegistrationModel registration)
    {
        ArgumentNullException.ThrowIfNull(registration);

        var now = DateTimeOffset.UtcNow;

        CurrentSession = new UserSession
        {
            SessionId = Guid.NewGuid(),
            Name = registration.Name.Trim(),
            Email = registration.Email.Trim(),
            StartedAtUtc = now,
            LastActivityAtUtc = now,
            IsActive = true
        };

        IsInitialized = true;

        await PersistAsync();
        NotifySessionChanged();
    }

    public async Task RegisterActivityAsync()
    {
        if (!IsActive || CurrentSession is null)
        {
            return;
        }

        CurrentSession.LastActivityAtUtc = DateTimeOffset.UtcNow;

        await PersistAsync();
        NotifySessionChanged();
    }

    public async Task EndAsync()
    {
        if (CurrentSession is not null)
        {
            CurrentSession.IsActive = false;
        }

        CurrentSession = null;
        IsInitialized = true;

        try
        {
            var storageModule = await GetModuleAsync();

            await storageModule.InvokeVoidAsync(
                "removeItem",
                StorageKey);
        }
        catch (InvalidOperationException)
        {
            // JavaScript todavía no está disponible.
        }
        catch (JSDisconnectedException)
        {
            // La conexión interactiva ya terminó.
        }

        NotifySessionChanged();
    }

    private async Task PersistAsync()
    {
        if (CurrentSession is null)
        {
            return;
        }

        var json = JsonSerializer.Serialize(
            CurrentSession,
            jsonOptions);

        var storageModule = await GetModuleAsync();

        await storageModule.InvokeVoidAsync(
            "setItem",
            StorageKey,
            json);
    }

    private async Task<IJSObjectReference> GetModuleAsync()
    {
        ObjectDisposedException.ThrowIf(isDisposed, this);

        module ??= await jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./js/sessionStorage.js");

        return module;
    }

    private void NotifySessionChanged()
    {
        SessionChanged?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        if (isDisposed)
        {
            return;
        }

        isDisposed = true;

        if (module is not null)
        {
            try
            {
                await module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // No es necesario liberar un módulo después
                // de desconectarse el circuito.
            }
        }
    }
}
