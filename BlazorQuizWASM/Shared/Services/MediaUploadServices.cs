using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BlazorQuizWASM.Client.Services
{
    public class MediaUploadServices
    {
        private readonly HttpClient _httpClient;
        private MediaUploadRequestDto mediaModel = new MediaUploadRequestDto();
        private readonly IFormFile? _formFile;

        public MediaUploadServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task UploadFileAsync(IBrowserFile browserFile)
        {
            bool success;
            string[] errors = { };
            //MudTextField<string>? pwField1;
            string? APIErrorMessage;

            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await mediaModel.File.OpenReadStream().CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            IFormFile formFile = new FormFile(new MemoryStream(fileData), 0, fileData.Length, mediaModel.File.Name, mediaModel.File.Name);

            var jsonPayload = JsonSerializer.Serialize(mediaModel);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/mediafiles/upload", requestContent);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                success = false;
                APIErrorMessage = "Error to fetch";
                throw new Exception(APIErrorMessage);
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                success = true;
            }



        }
    }
}
