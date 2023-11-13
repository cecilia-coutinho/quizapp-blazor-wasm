using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorQuizWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaFilesController : ControllerBase
    {
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly IMediaTypeRepository _mediaTypeRepository;

        public MediaFilesController(IMediaFileRepository mediaFileRepository, IMediaTypeRepository mediaTypeRepository)
        {
            _mediaFileRepository = mediaFileRepository;
            _mediaTypeRepository = mediaTypeRepository;
        }

        // POST: api/MediaFiles/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult> Upload([FromForm] MediaUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                var mediaTypeId = await GetMediaTypeIdFromRequest(request);

                if (mediaTypeId != Guid.Empty && request.File != null)
                {
                    //convert DTO to Domain Model
                    var mediaDomainModel = new MediaFile
                    {
                        FkMediaTypeId = mediaTypeId,
                        File = request.File,
                        FileExtension = Path.GetExtension(request.File.FileName).ToLower(),
                        FileSizeInBytes = request.File.Length,
                        MediaFileName = request.MediaFileName.ToLower(),
                    };
                    // User repository to uplaod files
                    await _mediaFileRepository.Upload(mediaDomainModel);

                    var customResponse = new MediaFileResponseDto
                    {
                        MediaFileName = mediaDomainModel.MediaFileName,
                        FileExtension = mediaDomainModel.FileExtension,
                        FileSizeInBytes = mediaDomainModel.FileSizeInBytes,
                        FilePath = mediaDomainModel.FilePath
                    };

                    return Ok(customResponse);
                }
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(MediaUploadRequestDto request)
        {
            var contentType = request?.File?.ContentType;
            var allowedExtensions = new string[] { "image/jpeg", "image/png", "video/mp4", "video/webm", "video/x-m4v" };

            if (!allowedExtensions.Contains(contentType))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request?.File?.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }

        private async Task<Guid> GetMediaTypeIdFromRequest(MediaUploadRequestDto request)
        {
            if (request.MediaType == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var mediaType = await _mediaTypeRepository.GetMediaType(request.MediaType);
            return mediaType?.MediaId ?? Guid.Empty;
        }
    }
}
