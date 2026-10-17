using FFMpegCore;
using FFMpegCore.Extensions.Downloader;

namespace MP4_to_Gif_Converter
{
    internal static class FFmpegHelper
    {
        private static readonly string folder = Path.Combine(AppContext.BaseDirectory, "ffmpeg"); // Folder to place FFmpeg in

        public static async Task<string> Download()
        {
            Directory.CreateDirectory(folder); // Creates the folder 

            GlobalFFOptions.Configure(options => options.BinaryFolder = folder);
            await FFMpegDownloader.DownloadBinaries(); // Downloads FFmpeg

            string path = Path.Combine(folder, "ffmpeg.exe"); // Creates executable path
            if (File.Exists(path))
            {
                return path; // Won't redownload if copy exists
            }

            return path;
        }
    }
}