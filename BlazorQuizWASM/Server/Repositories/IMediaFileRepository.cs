using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Shared.DTO;
using System.Drawing;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IMediaFileRepository
    {
        Task<MediaFile> Upload(MediaFile media);
        Task<MediaFile?> GetMedia(string MediaFileName);
    }
}
