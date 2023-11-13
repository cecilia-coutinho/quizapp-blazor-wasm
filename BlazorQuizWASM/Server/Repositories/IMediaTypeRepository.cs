using BlazorQuizWASM.Server.Models.Domain;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IMediaTypeRepository
    {
        Task<MediaType> GetMediaType(string mediaType);
    }
}
