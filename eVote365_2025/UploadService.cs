using Evote365.Core.Application.Interfaces;

namespace eVote365_2025
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _env;

        public UploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/" + fileName;
        }
    }
}
