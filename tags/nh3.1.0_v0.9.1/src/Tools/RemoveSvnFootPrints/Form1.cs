using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RemoveSvnFootPrints
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static void DeleteFileSystemInfo(FileSystemInfo fsi)
        {
            fsi.Attributes = FileAttributes.Normal;
            var di = fsi as DirectoryInfo;

            if (di != null)
            {
                foreach (var dirInfo in di.GetFileSystemInfos())
                    DeleteFileSystemInfo(dirInfo);
            }

            fsi.Delete();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = DirectoryTextBox.Text;

            if (!Directory.Exists(path))
            {
                MessageBox.Show("no such directory");
            }

            foreach (string directory in Directory.GetDirectories(path, ".svn", SearchOption.AllDirectories))
            {
                DeleteFileSystemInfo(new DirectoryInfo(directory));
            }

            MessageBox.Show("Svn foot prints removed!");
        }
    }
}
