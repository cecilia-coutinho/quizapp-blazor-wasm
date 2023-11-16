using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

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
        public async Task<ActionResult<List<MediaFileResponseDto>>> Upload([FromForm] IEnumerable<IFormFile> request)
        {
            List<MediaFileResponseDto> uploadResults = new List<MediaFileResponseDto>();

            foreach (var file in request)
            {
                ValidateFileUpload(file);

                if (ModelState.IsValid)
                {
                    var uploadResult = new MediaFileResponseDto();
                    string trustedFileNameForFileStorage;
                    var untrustedFileName = file.FileName;
                    uploadResult.MediaFileName = untrustedFileName;
                    var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                    trustedFileNameForFileStorage = Path.GetRandomFileName();
                    var fileExtension = Path.GetExtension(uploadResult.MediaFileName).ToLowerInvariant();

                    var mediaTypeId = await GetMediaTypeIdFromRequest(file);

                    if (mediaTypeId != Guid.Empty && request?.FirstOrDefault() != null)
                    {
                        //convert DTO to Domain Model
                        var mediaDomainModel = new MediaFile
                        {
                            FkMediaTypeId = mediaTypeId,
                            File = file,
                            FileExtension = fileExtension,
                            FileSizeInBytes = file.Length,
                            MediaFileName = trustedFileNameForFileStorage,
                        };
                        // User repository to uplaod files
                        await _mediaFileRepository.Upload(mediaDomainModel);

                        uploadResult.MediaFileName = trustedFileNameForFileStorage;
                        uploadResults.Add(uploadResult);

                        return Ok(uploadResults);
                    }
                }
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile request)
        {
            var contentType = request?.ContentType;
            var allowedExtensions = new string[] { "image/jpeg", "image/png", "video/mp4", "video/webm", "video/x-m4v" };

            if (!allowedExtensions.Contains(contentType))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request?.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }

        private async Task<Guid> GetMediaTypeIdFromRequest(IFormFile request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var allowedImageTypes = new string[] { "image/jpeg", "image/png" };
            var allowedVideoTypes = new string[] { "video/mp4", "video/webm", "video/x-m4v" };
            var contentType = request.ContentType;
            string? media = null;

            if (contentType != null && allowedImageTypes.Contains(contentType))
            {
                media = "image";
            }
            if (contentType != null && allowedVideoTypes.Contains(contentType))
            {
                media = "video";
            }

            if (media == null)
            {
                throw new Exception("Unsupported media type");
            }
            var mediaType = await _mediaTypeRepository.GetMediaType(media);
            return mediaType?.MediaId ?? Guid.Empty;
        }
    }
}
