#nullable enable
namespace EsnaMonitoring.Services.Configuations.IO
{
    using System.Threading.Tasks;

    public interface IFileReader
    {
        bool Exists(string path);

        Task<string?> ReadAllText(string path);

        Task WriteAllText(string path, string text);
    }
}