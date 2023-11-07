using BlazorQuizWASM.Server.Models.Domain;
using System.Drawing;

namespace BlazorQuizWASM.Server.Repositories
{
    public interface IMediaFileRepository
    {
        Task<MediaFile> Upload(MediaFile image);
    }
}
