using Microsoft.JSInterop;
using System.Text.Json;

namespace RecetaApp.Services;

public class UserInfo
{
    public string Uid { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class AuthResult
{
    public bool Success { get; set; }
    public UserInfo? User { get; set; }
    public string? Error { get; set; }
}

public class AuthService : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private DotNetObjectReference<AuthService>? _dotNetRef;
    private TaskCompletionSource<bool> _initializationComplete = new();

    public UserInfo? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;
    public bool IsInitialized { get; private set; }

    public event Action<UserInfo?>? AuthStateChanged;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (IsInitialized) return;

        await _jsRuntime.InvokeVoidAsync("waitForFirebase");

        _dotNetRef = DotNetObjectReference.Create(this);
        await _jsRuntime.InvokeVoidAsync("firebaseAuth.onAuthStateChanged", _dotNetRef);
    }

    public Task WaitForInitializationAsync()
    {
        return _initializationComplete.Task;
    }

    [JSInvokable("OnAuthStateChanged")]
    public void HandleAuthStateChanged(UserInfo? user)
    {
        CurrentUser = user;

        if (!IsInitialized)
        {
            IsInitialized = true;
            _initializationComplete.TrySetResult(true);
        }

        AuthStateChanged?.Invoke(user);
    }

    public async Task<AuthResult> SignInAsync(string email, string password)
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<JsonElement>("firebaseAuth.signIn", email, password);
            return ParseAuthResult(result);
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<AuthResult> SignUpAsync(string email, string password)
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<JsonElement>("firebaseAuth.signUp", email, password);
            return ParseAuthResult(result);
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<AuthResult> SignInWithGoogleAsync()
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<JsonElement>("firebaseAuth.signInWithGoogle");
            return ParseAuthResult(result);
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<AuthResult> SignOutAsync()
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<JsonElement>("firebaseAuth.signOut");
            if (result.GetProperty("success").GetBoolean())
            {
                CurrentUser = null;
                return new AuthResult { Success = true };
            }
            return new AuthResult { Success = false, Error = result.GetProperty("error").GetString() };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    private AuthResult ParseAuthResult(JsonElement result)
    {
        var success = result.GetProperty("success").GetBoolean();

        if (success)
        {
            var userElement = result.GetProperty("user");
            var user = new UserInfo
            {
                Uid = userElement.GetProperty("uid").GetString() ?? string.Empty,
                Email = userElement.GetProperty("email").GetString() ?? string.Empty
            };
            CurrentUser = user;
            return new AuthResult { Success = true, User = user };
        }

        return new AuthResult
        {
            Success = false,
            Error = result.GetProperty("error").GetString()
        };
    }

    public ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();
        return ValueTask.CompletedTask;
    }
}
