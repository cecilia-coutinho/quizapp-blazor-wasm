using BlazorQuizWASM.Shared.DTO;
using System.Net.Http.Json;

namespace BlazorQuizWASM.Shared.Services
{
    public class QuestionService
    {
        private readonly HttpClient _httpClient;
        private List<AnswersQuestionResponseDto>? questions;
        private string? errorMessage = "No questions here";

        public QuestionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AnswersQuestionResponseDto>> FetchQuestionsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Questions");

            if (response.IsSuccessStatusCode)
            {
                questions = await response.Content.ReadFromJsonAsync<List<AnswersQuestionResponseDto>>();

                if (questions == null)
                {
                    errorMessage = "There was an error deserializing the response!";
                    throw new Exception($"There was an error deserializing the response! {errorMessage}, \nStatusCode {response.StatusCode}, \nresponse Content {response.Content},  \nresponse Headers {response.Headers}  ");
                }
                else
                {
                    return questions;
                }
            }
            else
            {
                errorMessage = response.ReasonPhrase;
                throw new Exception($"There was an error in the response! {errorMessage}, \nStatusCode {response.StatusCode}, \nresponse Content {response.Content},  \nresponse Headers {response.Headers}  ");
            }
        }
    }
}
