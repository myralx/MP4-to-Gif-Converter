namespace MP4_to_Gif_Converter
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dropLabel = new Label();
            convertButton = new Button();
            SuspendLayout();
            // 
            // dropLabel
            // 
            dropLabel.AllowDrop = true;
            dropLabel.BackColor = SystemColors.Control;
            dropLabel.BorderStyle = BorderStyle.FixedSingle;
            dropLabel.Font = new Font("Arial Black", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dropLabel.ImageAlign = ContentAlignment.BottomCenter;
            dropLabel.Location = new Point(221, 89);
            dropLabel.Name = "dropLabel";
            dropLabel.Size = new Size(358, 156);
            dropLabel.TabIndex = 0;
            dropLabel.Text = "Drop your MP4 files here!";
            dropLabel.TextAlign = ContentAlignment.MiddleCenter;
            dropLabel.Click += label1_Click;
            // 
            // convertButton
            // 
            convertButton.Font = new Font("Arial Black", 8.25F);
            convertButton.Location = new Point(344, 349);
            convertButton.Name = "convertButton";
            convertButton.Size = new Size(112, 34);
            convertButton.TabIndex = 1;
            convertButton.Text = "Convert";
            convertButton.UseVisualStyleBackColor = true;
            convertButton.Click += convertButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(convertButton);
            Controls.Add(dropLabel);
            Name = "Form1";
            Text = "MP4 to Gif";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label dropLabel;
        private Button convertButton;
    }
}
