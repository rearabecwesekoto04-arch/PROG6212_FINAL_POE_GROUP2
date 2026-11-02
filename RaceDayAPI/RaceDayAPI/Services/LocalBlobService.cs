using Microsoft.AspNetCore.Http;
namespace RaceDayAPI.Services
{
    public class LocalBlobService : IBlobService
    {
        private readonly string _uploadDir;
        private readonly string _publicBaseUrl;

        public LocalBlobService(IWebHostEnvironment env, IConfiguration cfg, IHttpContextAccessor ctx)
        {
            // Save files under wwwroot/uploads/events/
            _uploadDir = Path.Combine(env.WebRootPath ?? "wwwroot", "uploads", "events");
            Directory.CreateDirectory(_uploadDir);

            // Prefer config override; fall back to the current request's host
            var configured = cfg["ApiSettings:PublicBaseUrl"];
            if (!string.IsNullOrWhiteSpace(configured))
            {
                _publicBaseUrl = configured.TrimEnd('/') + "/";
            }
            else
            {
                var req = ctx.HttpContext!.Request;
                _publicBaseUrl = $"{req.Scheme}://{req.Host}/uploads/events/";
            }
        }

        public async Task<string> UploadAsync(IFormFile file, string fileName)
        {
            var path = Path.Combine(_uploadDir, fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return $"{_publicBaseUrl}{fileName}";
        }

        public Task DeleteAsync(string fileName)
        {
            var path = Path.Combine(_uploadDir, fileName);
            if (File.Exists(path)) File.Delete(path);
            return Task.CompletedTask;
        }
    }
}