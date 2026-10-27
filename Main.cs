using System.Diagnostics;

namespace MP4_to_Gif_Converter

{
    public partial class Main : Form
    {
        private readonly List<string> selectedFiles = new();
        private string ffmpegPath = Path.Combine(AppContext.BaseDirectory, "ffmpeg", "ffmpeg.exe");

        public Main()
        {
            InitializeComponent();

            dropLabel.AllowDrop = true;
            dropLabel.DragEnter += dragEnter;
            dropLabel.DragDrop += dragDrop;

            this.AllowDrop = true;
            this.DragEnter += dragEnter;
            this.DragDrop += dragDrop;

            convertButton.Click += convert;
            Shown += checkFFmpeg;
        }

        private void addFiles(string[] files)
        {
            foreach (string file in files)
            {
                // Ignore other file types and already added files
                bool isMp4 = System.IO.File.Exists(file) &&
                    System.IO.Path.GetExtension(file).Equals(
                        ".mp4", StringComparison.OrdinalIgnoreCase);

                if (isMp4 &&
                    !selectedFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                {
                    selectedFiles.Add(file);
                }
            }

            dropLabel.Text = $"{selectedFiles.Count} MP4 file(s) selected";
            convertButton.Enabled = selectedFiles.Count > 0; // Enables button if files added
        }

        private async void checkFFmpeg(object? sender, EventArgs e)
        {
            if (File.Exists(ffmpegPath))
                return;

            // Prevent interaction during setup
            convertButton.Enabled = false;
            dropLabel.AllowDrop = false;
            this.AllowDrop = false;

            try
            {
                DialogResult choice = MessageBox.Show(
                    "Download Required",
                    "Download now?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (choice == DialogResult.No)
                {
                    Close();
                    return;
                }

                dropLabel.Text = "Downloading...";
                ffmpegPath = await FFmpegHelper.Download();

                dropLabel.Text = "Drop your MP4 files here!";
            }
            catch (Exception error)
            {
                dropLabel.Text = "FFmpeg download failed.";
                MessageBox.Show(error.Message, "Download failed");
            }
            finally
            {
                bool ready = File.Exists(ffmpegPath);
                
                dropLabel.AllowDrop = ready;
                this.AllowDrop = ready;
                convertButton.Enabled = ready && selectedFiles.Count > 0;
            }
        }

        private async void convert(object? sender, EventArgs e)
        {
            if (selectedFiles.Count == 0)
                return; // If no files added convert won't do anything

            // Disables adding mp4 / clicking convert whilst already converting
            convertButton.Enabled = false;
            dropLabel.AllowDrop = false;
            this.AllowDrop = false;

            UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor; // Uses the "loading" cursor

            // Runs the converter
            try
            {

                for (int i = 0; i < selectedFiles.Count; i++)
                {
                    dropLabel.Text = $"Converting {i + 1} of {selectedFiles.Count}...";

                    await Converter.ConvertFile(selectedFiles[i], ffmpegPath);
                }

                // Get the output folders before clearing the selection
                string[] folders = selectedFiles
                    .Select(file => Path.GetDirectoryName(file)!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                selectedFiles.Clear();
                dropLabel.Text = "Conversion successful!";

                // Open each output folder
                foreach (string folder in folders)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = folder,
                        UseShellExecute = true
                    });
                }
            }

            // Error handling
            catch (Exception error)
            {
                dropLabel.Text = "Conversion stopped.";
                MessageBox.Show(error.Message, "Conversion failed");
            }

            // Re-enables everything once conversion finished (or fails)
            finally
            {
                UseWaitCursor = false;
                Cursor.Current = Cursors.Default; // Sets cursor back to default

                dropLabel.AllowDrop = true;
                this.AllowDrop = true;
                convertButton.Enabled = selectedFiles.Count > 0;
            }

        }

        private void dragEnter(object? sender, DragEventArgs e)
        {
            // Check if file is mp4 file
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] files &&
                files.Any(file =>
                    System.IO.File.Exists(file) &&
                    System.IO.Path.GetExtension(file).Equals(
                        ".mp4", StringComparison.OrdinalIgnoreCase)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void dragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files)
                return;

            addFiles(files);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Prevent selecting files during setup or conversion
            if (!dropLabel.AllowDrop)
                return;

            using var picker = new OpenFileDialog
            {
                Title = "Select videos to convert",
                Filter = "MP4 videos (*.mp4)|*.mp4",
                Multiselect = true
            };

            if (picker.ShowDialog(this) == DialogResult.OK)
            {
                addFiles(picker.FileNames);
            }
        }

        private void convertButton_Click(object sender, EventArgs e)
        {

        }
    }
}
