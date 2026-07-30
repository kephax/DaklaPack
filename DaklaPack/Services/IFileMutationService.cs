namespace DaklaPack.Services
{
    public interface IFileMutationService
    {
        Task<Stream> MutateFileAsync(Stream input, CancellationToken cancellationToken = default);
    }
}
