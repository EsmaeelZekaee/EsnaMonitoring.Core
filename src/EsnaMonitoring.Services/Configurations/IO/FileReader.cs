#nullable enable
namespace EsnaMonitoring.Services.Configuations.IO
{
    using System.IO;
    using System.Threading.Tasks;

    public class FileReader : IFileReader
    {
        public bool Exists(string path)
        {
            return File.Exists(PathHelper.GetPath(path));
        }

        public async Task<string?> ReadAllText(string path)
        {
            return await File.ReadAllTextAsync(PathHelper.GetPath(path));
        }
        public Task WriteAllText(string path, string text)
        {
            File.WriteAllText(PathHelper.GetPath(path), text);
            return Task.CompletedTask;
        }
    }
}
