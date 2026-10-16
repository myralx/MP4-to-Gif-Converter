namespace MP4_to_Gif_Converter
{
    public partial class Main : Form
    {
        private readonly List<string> selectedFiles = new();

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
        }

        private async void convert(object? sender, EventArgs e)
        {
            if (selectedFiles.Count == 0)
                return; // If no files added convert won't do anything

            // Disables adding mp4 / clicking convert whilst already converting
            convertButton.Enabled = false;
            dropLabel.AllowDrop = false;
            this.AllowDrop = false;

            // Runs the converter
            try 
            {
                for (int i = 0; i < selectedFiles.Count; i++)
                {
                    dropLabel.Text = $"Converting {i + 1} of {selectedFiles.Count}...";

                    await Converter.ConvertFile(selectedFiles[i]);
                }

                selectedFiles.Clear();
                dropLabel.Text = "Conversion successful!";
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void convertButton_Click(object sender, EventArgs e)
        {

        }
    }
}
