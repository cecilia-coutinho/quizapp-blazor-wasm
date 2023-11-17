using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;

namespace BlazorQuizWASM.Client.Services
{
    public class QuizService
    {
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly HttpClient _httpClient;

        public QuizService(IAccessTokenProvider tokenProvider, HttpClient httpClient)
        {
            _tokenProvider = tokenProvider;
            _httpClient = httpClient;
        }

        public async Task RequestAuthToken()
        {
            var requestToken = await _tokenProvider.RequestAccessToken();
            requestToken.TryGetToken(out var token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }

    }

}
