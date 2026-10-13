namespace MP4_to_Gif_Converter
{
    public partial class Form1 : Form
    {
        private readonly List<string> selectedFiles = new();

        public Form1()
        {
            InitializeComponent();

            dropLabel.AllowDrop = true;
            dropLabel.DragEnter += dragEnter;
            dropLabel.DragDrop += dragDrop;

            this.AllowDrop = true;
            this.DragEnter += dragEnter;
            this.DragDrop += dragDrop;
        }

        private void dragEnter(object? sender, DragEventArgs e)
    {   
            // Check is file is mp4 file
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
    }
}
