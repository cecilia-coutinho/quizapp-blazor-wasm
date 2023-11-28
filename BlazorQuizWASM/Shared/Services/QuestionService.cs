using BlazorQuizWASM.Shared.DTO;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BlazorQuizWASM.Shared.Services
{
    public class QuestionService
    {
        private readonly HttpClient _http;
        private List<AnswersQuestionResponseDto>? questions;
        private string? errorMessage = "No questions here";

        public QuestionService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<AnswersQuestionResponseDto>> FetchQuestionsAsync()
        {
            var response = await _http.GetAsync("/api/Questions");

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

        public async Task<List<AnswersQuestionResponseDto>> FetchQuestionsWithAnswersAsync()
        {
            var response = await _http.GetAsync("/api/Questions/questions-with-answers");

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

        public async Task<List<AnswersQuestionResponseDto>> PublishQuestionAsync(string questionPath)
        {
            var response = await _http.PutAsJsonAsync($"/api/Questions/question/{questionPath}", questionPath);

            if (response.IsSuccessStatusCode)
            {
                questions = await FetchQuestionsWithAnswersAsync();
                return questions;
            }
            else
            {
                errorMessage = response.ReasonPhrase;
                throw new Exception($"There was an error in the response! {errorMessage}, \nStatusCode {response.StatusCode}, \nresponse Content {response.Content},  \nresponse Headers {response.Headers}  ");
            }
        }

        public async Task<List<AnswersQuestionResponseDto>> DeleteQuestionAsync(string questionPath)
        {
            var response = await _http.DeleteAsync($"/api/Questions/question/{questionPath}");

            if (response.IsSuccessStatusCode)
            {
                questions = await FetchQuestionsWithAnswersAsync();
                return questions;
            }
            else
            {
                errorMessage = response.ReasonPhrase;
                throw new Exception($"There was an error in the response! {errorMessage}, \nStatusCode {response.StatusCode}, \nresponse Content {response.Content},  \nresponse Headers {response.Headers}  ");
            }
        }

    }
}
