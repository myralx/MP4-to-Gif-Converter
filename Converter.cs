using System.Diagnostics;

namespace MP4_to_Gif_Converter
{
    internal static class Converter
    {
        public static async Task ConvertFile(string input)
        {
            string ffmpegPath = "ffmpeg.exe";

            string output = Path.ChangeExtension(input, ".gif");

            // Prevent duplicate file names / overwriting
            int number = 1; // Start filenames at 1
            while (File.Exists(output))
            {
                output = Path.Combine(
                    Path.GetDirectoryName(input)!,
                    $"{Path.GetFileNameWithoutExtension(input)} ({number}).gif");
                number++; // adds +1 if name exists
            }

            // FFmpeg filters
            string filter =
                "fps=30,scale=800:-1:flags=lanczos,split[s0][s1];" +
                "[s0]palettegen=max_colors=256[p];" +
                "[s1][p]paletteuse=dither=sierra2_4a";

            var startInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true
            };

            // FFmpeg settings
            foreach (string argument in new[]
            {"-nostdin","-n","-i", input,"-vf", filter,output})

            {
                startInfo.ArgumentList.Add(argument);
            }

            // Creates process object for Ffmpeg
            using var process = new Process { StartInfo = startInfo };

            process.Start(); // Starts FFmpeg

            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync(); // Wait for FFmpeg to finish

            if (process.ExitCode != 0) // Error handling
            {
                throw new Exception($"Could not convert {Path.GetFileName(input)}:\n{error}");
            }
        }
    }
}