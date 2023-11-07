﻿using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using System.Drawing;

namespace BlazorQuizWASM.Server.Repositories
{
    public class LocalMediaFileRepository : IMediaFileRepository
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _dbContext;

        public LocalMediaFileRepository(IWebHostEnvironment environment, IHttpContextAccessor contextAccessor, ApplicationDbContext dbContext)
        {
            _environment = environment;
            _contextAccessor = contextAccessor;
            _dbContext = dbContext;
        }

        public async Task<MediaFile> Upload(MediaFile media)
        {
            var localFilePath = Path.Combine(_environment.ContentRootPath, "Medias", $"{media.MediaFileName}{media.FileExtension}");

            if (media.MediaFileName.Length > 0 && media.File != null)
            {
                // Upload Media to Local Path
                using var stream = new FileStream(localFilePath, FileMode.Create);
                await media.File.CopyToAsync(stream);
            }

            // https://localhost:1234/images/image.jpg
            var urlFilePath = $"{_contextAccessor?.HttpContext?.Request.Scheme}:" +
                $"//{_contextAccessor?.HttpContext?.Request.Host}" +
                $"{_contextAccessor?.HttpContext?.Request.PathBase}" +
                $"/Medias/{media.MediaFileName}{media.FileExtension}";

            media.FilePath = urlFilePath;

            if (_dbContext.MediaFiles != null)
            {
                //add Media to Media Table
                await _dbContext.MediaFiles.AddAsync(media);
                await _dbContext.SaveChangesAsync();
            }

            return media;
        }
    }
}
