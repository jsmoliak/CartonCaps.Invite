using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.Model.ValueObjects;
using System.Text.Json;

namespace CartonCaps.Invite.API.Infrastructure
{
    /// <summary>
    /// A client for interacting with the profile management API.
    /// </summary>
    public class ProfileManagementApiClient : IProfileManagementApiClient
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileManagementApiClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient factory.</param>
        public ProfileManagementApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Retrieves a user profile by user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A task representing the asynchronous operation, returning the user profile.</returns>
        /// <exception cref="ProfileManagementException">Thrown when the profile retrieval fails.</exception>
        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"https://api.example.com/profiles/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserProfile>(content, _jsonOptions) ?? throw new ProfileManagementException();
            }
            throw new ProfileManagementException();
        }
    }
}
