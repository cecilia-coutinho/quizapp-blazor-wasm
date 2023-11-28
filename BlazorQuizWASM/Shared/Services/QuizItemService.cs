using BlazorQuizWASM.Shared.DTO;
using System.Net.Http.Json;

namespace BlazorQuizWASM.Shared.Services
{
    public class QuizItemService
    {
        private readonly HttpClient _httpClient;
        private List<QuizItemQuestionResponseDto>? quizItems;

        public QuizItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<QuizItemQuestionResponseDto>> FetchParticipantsAsync()
        {
            var response = await _httpClient.GetAsync($"api/QuizItems");

            if (response.IsSuccessStatusCode)
            {
                quizItems = await response.Content.ReadFromJsonAsync<List<QuizItemQuestionResponseDto>>();

                if (quizItems == null)
                {
                    throw new Exception("There was an error fetching the quizItems or the quizItems were null");
                }
                return quizItems;
            }
            else
            {
                throw new Exception($"There was an error in the response! {response.ReasonPhrase}, \nStatusCode {response.StatusCode}, \nresponse Content {response.Content},  \nresponse Headers {response.Headers}");
            }
        }
    }
}
