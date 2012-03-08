namespace RemoveSvnFootPrints
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DirectoryTextBox = new System.Windows.Forms.TextBox();
            this.RemoveSvnFootPrintsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DirectoryTextBox
            // 
            this.DirectoryTextBox.Location = new System.Drawing.Point(12, 12);
            this.DirectoryTextBox.Name = "DirectoryTextBox";
            this.DirectoryTextBox.Size = new System.Drawing.Size(269, 20);
            this.DirectoryTextBox.TabIndex = 0;
            // 
            // RemoveSvnFootPrintsButton
            // 
            this.RemoveSvnFootPrintsButton.Location = new System.Drawing.Point(288, 12);
            this.RemoveSvnFootPrintsButton.Name = "RemoveSvnFootPrintsButton";
            this.RemoveSvnFootPrintsButton.Size = new System.Drawing.Size(139, 23);
            this.RemoveSvnFootPrintsButton.TabIndex = 1;
            this.RemoveSvnFootPrintsButton.Text = "Remove Svn Foot Prints";
            this.RemoveSvnFootPrintsButton.UseVisualStyleBackColor = true;
            this.RemoveSvnFootPrintsButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 44);
            this.Controls.Add(this.RemoveSvnFootPrintsButton);
            this.Controls.Add(this.DirectoryTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DirectoryTextBox;
        private System.Windows.Forms.Button RemoveSvnFootPrintsButton;
    }
}

