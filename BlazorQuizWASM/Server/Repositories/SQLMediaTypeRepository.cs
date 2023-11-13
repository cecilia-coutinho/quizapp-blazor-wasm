using BlazorQuizWASM.Server.Data;
using BlazorQuizWASM.Server.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorQuizWASM.Server.Repositories
{
    public class SQLMediaTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public SQLMediaTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<MediaType?> GetMediaType(string mediaType)
        {
            if (_context.MediaTypes == null)
            {
                throw new Exception("Entity 'MediaFiles' not found.");
            }

            var media = await _context.MediaTypes.FirstOrDefaultAsync(mt => mt.Mediatype == mediaType);

            return media;
        }
    }
}
