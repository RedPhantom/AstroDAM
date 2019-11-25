using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroDAM
{
    public partial class frmFileImporter : Form
    {
        public DialogRes DialogRes { get; set; }

        FileTypes FileType;

        public enum FileTypes
        {
            CaptureFile,
            MetadataFile
        }

        public frmFileImporter(FileTypes fileType)
        {
            InitializeComponent();

            FileType = fileType;
        }

        public void FileChecker()
        {
            bool res;

            res = File.Exists(tbFilePath.Text);

            if (res)
            {
                lblFileResult.Text = "File located successfully.";
                btnSave.Enabled = true;
            } else
            {
                lblFileResult.Text = "Failed to locate file.";
                btnSave.Enabled = false;
            }
        }

        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            DialogResult = openFileDialog1.ShowDialog();

            if (DialogResult == DialogResult.OK)
            {
                tbFilePath.Text = openFileDialog1.FileName;
            }

            FileChecker();
        }

        private void tbFilePath_TextChanged(object sender, EventArgs e)
        {
            FileChecker();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            DialogRes = new DialogRes(tbFilePath.Text, cbImportData.Checked);
            DialogRes.Metadata = new Dictionary<string, string>();

            if (FileType == FileTypes.MetadataFile)
            {
                string[] metadataLines = File.ReadAllLines(tbFilePath.Text);
                string[] line;
                List<string> unparsableLines = new List<string>();

                for (int i = 0; i < metadataLines.Length; i++)
                {
                    if (metadataLines[i].Contains("="))
                    {
                        line = metadataLines[i].Split('=');
                        DialogRes.Metadata.Add(line[0], line[1]);
                    } else
                    {
                        unparsableLines.Add(metadataLines[i]);
                    }
                }
            }

            Close();
        }
    }

    public class DialogRes
    {
        public DialogRes() { }

        public DialogRes(string filePath, bool importFileMetadata)
        {
            FilePath = filePath;
            ImportFileMetadata = importFileMetadata;
        }

        public string FilePath { get; set; }
        public bool ImportFileMetadata { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string[] UnparsableLines { get; set; }
    }
}
