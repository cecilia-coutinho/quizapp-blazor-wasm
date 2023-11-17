using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Server.Repositories;
using BlazorQuizWASM.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [Authorize]
        public async Task<ActionResult> Upload([FromForm] MediaUploadRequestDto request)
        {

            if (request == null || request?.File?.Length == 0)
            {
                ModelState.AddModelError("file", "No file uploaded.");
                return BadRequest("No file uploaded.");
            }

            ValidateFileUpload(request.File);
            List<MediaFileResponseDto> uploadResults = new List<MediaFileResponseDto>();

            if (ModelState.IsValid)
            {
                var uploadResult = new MediaFileResponseDto();

                var untrustedFileName = request.File.FileName;
                uploadResult.MediaFileName = untrustedFileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();

                var mediaTypeId = await GetMediaTypeIdFromRequest(request.File);

                if (mediaTypeId != Guid.Empty && request != null)
                {
                    var mediaDomainModel = new MediaFile
                    {
                        FkMediaTypeId = mediaTypeId,
                        File = request.File,
                        FileExtension = fileExtension,
                        FileSizeInBytes = request.File.Length,
                        MediaFileName = trustedFileNameForFileStorage,
                    };

                    await _mediaFileRepository.Upload(mediaDomainModel);

                    uploadResult.MediaFileName = trustedFileNameForDisplay;
                    uploadResult.StoredFileName = trustedFileNameForFileStorage;
                    uploadResults.Add(uploadResult);

                    return Ok(uploadResults);
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
                ModelState.AddModelError("file","Unsupported media type");
            }
            var mediaType = await _mediaTypeRepository.GetMediaType(media);
            return mediaType?.MediaId ?? Guid.Empty;
        }
    }
}
