using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroDAM
{
    public partial class frmManager : Form
    {
        BackgroundWorker bgwDataLoader = new BackgroundWorker();

        public enum ManagerTabs
        {
            Cameras,
            Catalogues,
            ColorSpaces,
            FileFormats,
            Optics,
            Photographers,
            Scopes,
            Sites
        }

        public frmManager(ManagerTabs Tab)
        {
            InitializeComponent();

            bgwDataLoader.DoWork += BgwDataLoader_DoWork;
            bgwDataLoader.RunWorkerCompleted += BgwDataLoader_RunWorkerCompleted;

            tabControl1.SelectedIndex = (int)Tab;
            bgwDataLoader.RunWorkerAsync();
        }

        private void BgwDataLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void BgwDataLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void ShowPreparing()
        {
            pnlPreparing.Location = new Point(
                ClientSize.Width / 2 - pnlPreparing.Size.Width / 2,
                ClientSize.Height / 2 - pnlPreparing.Size.Height / 2);
            pnlPreparing.Anchor = AnchorStyles.None;

            pnlPreparing.Visible = true;
        }

        void HidePreparing()
        {
            pnlPreparing.Visible = false;
        }
    }
}
