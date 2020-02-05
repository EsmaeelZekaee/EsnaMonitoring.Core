namespace EsnaMonitoring.Services.Configuations.IO
{
    using System.IO;

    public static class PathHelper
    {
        public static string GetPath(string file)
        {
            return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, file);
        }
    }
}
