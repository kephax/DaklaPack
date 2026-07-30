using System.Reflection.PortableExecutable;
using System.Text;

namespace DaklaPack.Services
{
    public class FileMutationService : IFileMutationService
    {
        /// <summary>
        /// Mutates the provided file stream by adding a timestamp and a random character sequence.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Stream> MutateFileAsync(Stream input, CancellationToken cancellationToken = default)
        {
            string randomCharachterSequence = GenerateRandomString(20); // 20 is the length of the random string
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");

            // Before processing the file verify (again) it's a text file
            if (!IsTextFile(input))
            {
                throw new InvalidOperationException("The provided stream is not a text file.");
            }

            // Vefify the file size is less than 1MB. This to prevent large files from being processed and overload the system
            if (!IsFileSizeValid(input))
            {
                throw new InvalidOperationException("The provided stream exceeds the maximum allowed size of 10 MiB.");
            }

            // Proceed with the mutation if the file is valid
            using var reader = new StreamReader(input, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
            var originalContent = await reader.ReadToEndAsync(cancellationToken);

            var mutatedContent = $"{originalContent}{Environment.NewLine}" +
                                 $"DateTime:{timestamp}{Environment.NewLine}" +
                                 $"RandomString:{randomCharachterSequence}";

            var output = new MemoryStream(Encoding.UTF8.GetBytes(mutatedContent));
            output.Position = 0;
            return output;
        }

        /// <summary>
        /// Generates a random string of the specified length using alphanumeric characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Add more characters if needed
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Validates if the provided stream's size is within the allowed limit (10 MiB).
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool IsFileSizeValid(Stream stream)
        {
            const long maxSizeInBytes = 10 * 1024 * 1024; // 10 MiB - Change to appropriate size limit if needed
            if (stream.Length > maxSizeInBytes)
            {
                return false;

            }
            return true;
        }

        /// <summary>
        /// Checks if the provided stream is a text file by examining its content.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool IsTextFile(Stream stream)
        {
            // Check for common text file signatures (e.g., UTF-8 BOM)
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            stream.Seek(0, SeekOrigin.Begin); // Reset the stream position after reading
            return !(buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF);
        }
    }
}
