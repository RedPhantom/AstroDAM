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

        public enum ObjectTypes
        {
            File,
            Directory
        }

        public frmFileImporter(FileTypes fileType)
        {
            InitializeComponent();

            FileType = fileType;

            switch (fileType)
            {
                case FileTypes.CaptureFile:


                    break;
                case FileTypes.MetadataFile:
                    btnSearchFolder.Enabled = false;
                    OpenFileDialog(); // we want to save the user clicks. 
                    break;
                default:
                    break;
            }
        }

        public void FileChecker()
        {
            bool res;
            ObjectTypes objectType = GetObjectType(tbFilePath.Text);

            if (objectType == ObjectTypes.File)
            {
                res = File.Exists(tbFilePath.Text);
            }
            else
            {
                if (FileType == FileTypes.MetadataFile)
                {
                    lblFileResult.Text = "Cannot define a folder as a metadata file.";
                    btnSave.Enabled = false;
                    return;
                }
                else
                {
                    res = Directory.Exists(tbFilePath.Text);
                }
            }

            if (res)
            {
                lblFileResult.Text = objectType.ToString() + " located successfully.";
                btnSave.Enabled = true;
            }
            else
            {
                lblFileResult.Text = "Failed to locate file.";
                btnSave.Enabled = false;
            }
        }

        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog();
        }

        void OpenFileDialog()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbFilePath.Text = openFileDialog1.FileName;

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

            DialogRes dialogRes = new DialogRes(tbFilePath.Text, cbImportData.Checked);
            dialogRes.Metadata = new Dictionary<string, string>();

            List<string> unparsableLines = new List<string>();

            if (FileType == FileTypes.MetadataFile)
            {
                // basic text processing for metadata parsing.
                string[] metadataLines = File.ReadAllLines(tbFilePath.Text);
                string[] line;

                for (int i = 0; i < metadataLines.Length; i++)
                {
                    if (metadataLines[i].Contains("="))
                    {
                        line = metadataLines[i].Split('=');
                        dialogRes.Metadata.Add(line[0], line[1]);
                    }
                    else
                    {
                        unparsableLines.Add(metadataLines[i]);
                    }
                }

                dialogRes.UnparsableLines = unparsableLines.ToArray();
            }
            else
            {
                // we only send file/folder path and whether to analyze this file or not.
                dialogRes = new DialogRes(tbFilePath.Text, cbImportData.Checked);
            }

            DialogRes = dialogRes;
            Close();
        }

        // TODO nullable?
        public static ObjectTypes GetObjectType(string path)
        {
            if (string.IsNullOrEmpty(path))
                return ObjectTypes.File;

            try
            {
                FileAttributes attr = File.GetAttributes(path);

                if (attr.HasFlag(FileAttributes.Directory))
                    return ObjectTypes.Directory;
                else
                    return ObjectTypes.File;
            }
            catch (Exception)
            {
                return ObjectTypes.File;
            }
        }

        private void btnSearchFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                tbFilePath.Text = folderBrowserDialog1.SelectedPath;

            FileChecker();
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
