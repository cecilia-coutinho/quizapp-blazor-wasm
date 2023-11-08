using BlazorQuizWASM.Server.Data;
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
        private readonly ApplicationDbContext _context;

        private readonly IMediaFileRepository _mediaFileRepository;

        public MediaFilesController(IMediaFileRepository mediaFileRepository, ApplicationDbContext context)
        {
            _context = context;
            _mediaFileRepository = mediaFileRepository;
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
            if (_context.MediaTypes == null)
            {
                throw new Exception("_context.MediaTypes is null. Handle this case appropriately.");
            }

            if (request.MediaType == "image")
            {
                var media = await _context.MediaTypes.SingleOrDefaultAsync(mt => mt.Mediatype == "image");

                return media?.MediaId ?? Guid.Empty;

            }
            else if (request.MediaType == "video")
            {
                var media = await _context.MediaTypes.SingleOrDefaultAsync(mt => mt.Mediatype == "Video");

                return media?.MediaId ?? Guid.Empty;
            }
            else
            {
                throw new ArgumentException("Invalid media type in request");
            }
            
        }
    }
}
