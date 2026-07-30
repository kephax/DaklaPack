using DaklaPack.Services;
using Microsoft.AspNetCore.Mvc;

namespace DaklaPack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileMutationService _mutationService;

        public FilesController(IFileMutationService mutationService)
        {
            _mutationService = mutationService;
        }


        /// <summary>
        /// Mutates the uploaded text file by adding a timestamp and a random character sequence, then returns the mutated file for download.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("mutate")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10 MiB limit
        public async Task<IActionResult> MutateFile(IFormFile file, CancellationToken cancellationToken)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("No file uploaded or the file is empty.");
            }

            // Verify it's a text file
            if(!IsTextFile(file))
            {
                return BadRequest("Only text files are supported.");
            }

            // Execute
            await using var inputStream = file.OpenReadStream();
            var mutatedStream = await _mutationService.MutateFileAsync(inputStream, cancellationToken);

            var downloadFileName = $"Mutated_{Path.GetFileNameWithoutExtension(file.FileName)}.txt"; // Improvement would be to use the original file extension

            return File(mutatedStream, "text/plain", downloadFileName);
        }

        /// <summary>
        /// Checks if the uploaded file is a text file based on its content type and extension.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsTextFile(IFormFile file)
        {
            if (!file.ContentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase) &&
               !Path.GetExtension(file.FileName).Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }
    }
}
