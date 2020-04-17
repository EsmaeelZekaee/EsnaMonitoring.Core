namespace EsnaMonitoring.Services.Configuations.IO
{
    using System;
    using System.IO;

    public static class PathHelper
    {
        public static string GetPath(string file)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
        }
    }
}