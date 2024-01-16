using System.Net.Http.Headers;

using Data.Exceptions;

namespace API.Utility;

public interface IClerkClient
{
    Task<string?> CreateInvitation(string email, string role);
    Task<bool> DeleteUser(string id);
    Task<bool> DeleteUserByEmail(string email);
    Task<string?> GetInvitationIdByEmail(string email);
    Task<string?> GetUserIdByEmail(string email);
    Task<bool> RevokeInvitation(string id);
    Task<bool> RevokeInvitationByEmail(string email);
}

public class ClerkClient : IClerkClient
{
    private readonly HttpClient _httpClient;

    public ClerkClient(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;

        var uri = configuration["Clerk:ApiUri"] ?? throw new MissingValueException("Clerk:ApiUri");
        _httpClient.BaseAddress = new Uri(uri);

        var secretKey = configuration["Clerk:SecretKey"] ?? throw new MissingValueException("Clerk:SecretKey");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
    }

    public async Task<string?> CreateInvitation(string email, string role)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/invitations", new
        {
            email_address = email,
            public_metadata = new { role }
        });
        response.EnsureSuccessStatusCode();

        var invitation = await response.Content.ReadFromJsonAsync<ClerkInvitation>();
        return invitation?.id;
    }

    public async Task<bool> RevokeInvitation(string id)
    {
        var response = await _httpClient.PostAsync($"v1/invitations/{id}/revoke", null);
        response.EnsureSuccessStatusCode();

        var invitation = await response.Content.ReadFromJsonAsync<ClerkInvitation>();
        return invitation?.revoked ?? false;
    }

    public async Task<bool> RevokeInvitationByEmail(string email)
    {
        var id = await GetInvitationIdByEmail(email);
        return id is null || await RevokeInvitation(id);
    }

    public async Task<string?> GetInvitationIdByEmail(string email)
    {
        var response = await _httpClient.GetAsync("v1/invitations");
        response.EnsureSuccessStatusCode();

        var invitations = await response.Content.ReadFromJsonAsync<List<ClerkInvitation>>();
        return invitations?.Find(i => i.email_address == email)?.id;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var response = await _httpClient.DeleteAsync($"v1/users/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ClerkDelete>();
        return content?.deleted ?? false;
    }

    public async Task<bool> DeleteUserByEmail(string email)
    {
        var id = await GetUserIdByEmail(email);
        return id is null || await DeleteUser(id);
    }

    public async Task<string?> GetUserIdByEmail(string email)
    {
        var request = $"v1/users?limit=10&offset=0&order_by=-created_at&email_address={email}";
        var response = await _httpClient.GetAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<List<ClerkUser>>();

        return content?.FirstOrDefault()?.id ?? null;
    }
}
