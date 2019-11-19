using AstroDAM.Models;
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
        //BackgroundWorker bgwDataLoader = new BackgroundWorker();

        List<Camera> listCameras;
        List<Catalogue> listCatalogues;
        List<ColorSpace> listColorSpaces;
        List<FileFormat> listFileFormats;
        List<Optic> listOptics;
        List<Photographer> listPhotographers;
        List<Scope> listScopes;
        List<Site> listSites;

        EditingModes emCameras = EditingModes.Add;
        EditingModes emCatalogues = EditingModes.Add;
        EditingModes emColorSpaces = EditingModes.Add;
        EditingModes emFileFormats = EditingModes.Add;
        EditingModes emOptics = EditingModes.Add;
        EditingModes emPhotographers = EditingModes.Add;
        EditingModes emScopes = EditingModes.Add;
        EditingModes emSites = EditingModes.Add;

        Camera currentCamera = new Camera();
        Catalogue currentCatalogue = new Catalogue();
        ColorSpace currentColorSpace = new ColorSpace();
        FileFormat currentFileFormat = new FileFormat();
        Optic currentOptic = new Optic();
        Photographer currentPhotographer = new Photographer();
        Scope currentScope = new Scope();
        Site currentSite = new Site();

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

        public enum EditingModes
        {
            Add,
            Edit
        }

        public frmManager(ManagerTabs Tab)
        {
            InitializeComponent();

            //bgwDataLoader.DoWork += BgwDataLoader_DoWork;
            //bgwDataLoader.RunWorkerCompleted += BgwDataLoader_RunWorkerCompleted;

            tabControl1.SelectedIndex = (int)Tab;
            //bgwDataLoader.RunWorkerAsync();
        }

        private void frmManager_Load(object sender, EventArgs e)
        {
            // Populate all hard-coded combo-box values.
            foreach (var item in Enum.GetValues(typeof(Optic.OpticTypes)))
            {
                cbOpticType.Items.Add(item);
            }

            foreach (var item in Enum.GetValues(typeof(Scope.MountTypes)))
            {
                cbScopeMountType.Items.Add(item);
            }

            foreach (var item in Enum.GetValues(typeof(Site.LongtitudeTypes)))
            {
                cbSiteLongtitudeType.Items.Add(item);
            }

            foreach (var item in Enum.GetValues(typeof(Site.LatitudeTypes)))
            {
                cbSiteLatitudeType.Items.Add(item);
            }

            // Query database for all values.
            listCameras = Operations.GetCameras();
            listCatalogues = Operations.GetCatalogues();
            listColorSpaces = Operations.GetColorSpaces();
            listFileFormats = Operations.GetFileFormats();
            listOptics = Operations.GetOptics();
            listPhotographers = Operations.GetPhotographers();
            listScopes = Operations.GetScopes();
            listSites = Operations.GetSites();

            // Populate list boxes.
            foreach (var item in listCameras)
                lbCameras.Items.Add(item.Id + " " + item.LongName + " " + (item.MaxResolution.Width * item.MaxResolution.Height / 1000000).ToString() + "MP");

            foreach (var item in listCatalogues)
                lbCatalogues.Items.Add(item.Id + " " + item.LongName);

            foreach (var item in listColorSpaces)
                lbColorSpaces.Items.Add(item.Id + " " + item.Name);

            foreach (var item in listFileFormats)
                lbFileFormats.Items.Add(item.Id + " " + item.LongName);

            foreach (var item in listOptics)
                lbOptics.Items.Add(item.Id + " " + item.Value + " " + item.OpticType.ToString());

            foreach (var item in listPhotographers)
                lbPhotographers.Items.Add(item.Id + " " + item.LastName + ", " + item.FirstName);

            foreach (var item in listScopes)
                lbScopes.Items.Add(item.Id + " " + item.Manufacturer + " " + item.Name);

            foreach (var item in listSites)
                lbSites.Items.Add(item.Id + " " + item.Name);

            // Populate specific database fields for every tab.

            foreach (var item in listColorSpaces)
                cbCamerasColorSpaces.Items.Add(item.Name);
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

        private void UpdateEditingMode(EditingModes em, ref EditingModes editingModeVariable, ref Label tabLabel, string tabLabelText)
        {
            switch (em)
            {
                case EditingModes.Add:
                    editingModeVariable = EditingModes.Add;
                    tabLabel.Text = tabLabelText + " (adding)";

                    break;
                case EditingModes.Edit:
                    editingModeVariable = EditingModes.Edit;
                    tabLabel.Text = tabLabelText + " (editing)";

                    break;
                default:
                    break;
            }
        }

        // Cameras
        private void btnCamerasSave_Click(object sender, EventArgs e)
        {
            // Valiations:

            if (!Operations.IsDigitsOnly(tbCamerasXResolution.Text))
            {
                errorProvider.SetError(tbCamerasXResolution, "Must be a number.");
                return;
            }
            else
                errorProvider.SetError(tbCamerasXResolution, "");

            if (!Operations.IsDigitsOnly(tbCamerasYResolution.Text))
            {
                errorProvider.SetError(tbCamerasYResolution, "Must be a number.");
                return;
            }
            else
                errorProvider.SetError(tbCamerasYResolution, "");

            if (tbCamerasShortName.Text.Length == 0)
            {
                errorProvider.SetError(tbCamerasShortName, "Must provide a value.");
                return;
            }
            else
                errorProvider.SetError(tbCamerasYResolution, "");

            if (lbCamerasColorSpaces.Items.Count == 0)
            {
                errorProvider.SetError(lbCamerasColorSpaces, "Must specify at least one color space.");
                return;
            }

            // Save new/changes:

            int id = 0;
            string shortName = tbCamerasShortName.Text;
            string longName = tbCamerasLongName.Text;
            Size maxResolution = new Size(int.Parse(tbCamerasXResolution.Text), int.Parse(tbCamerasYResolution.Text));

            List<ColorSpace> colorSpaces = new List<ColorSpace>();
            foreach (var item in lbCamerasColorSpaces.Items)
            {
                ColorSpace cs = listColorSpaces.Find(x => x.Name == item.ToString());

                if (cs != null)
                    colorSpaces.Add(cs);
            }

            if (emCameras == EditingModes.Add)
                id = 0;
            else
                id = currentCamera.Id;

            Camera candidate = new Camera(id, shortName, longName, maxResolution, colorSpaces);

            if (emCameras == EditingModes.Add)
            
                id = Operations.AddCamera(candidate);
            else
                Operations.EditCamera(id, candidate);
           
            // Reload data:
            
            currentCamera = Operations.GetCameras(new List<int>() { id })[0];
            LoadCameraData(currentCamera);
        }

        private void btnCamerasNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emCameras, ref lblCameras, "Cameras");

            ClearCameraData();
        }

        private void lbCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emCameras, ref lblCameras, "Cameras");

            currentCamera = listCameras[lbCameras.SelectedIndex];
            LoadCameraData(currentCamera);
        }
        
        private void LoadCameraData(Camera camera)
        {

            tbCamerasId.Text = camera.Id.ToString();
            tbCamerasShortName.Text = camera.ShortName;
            tbCamerasLongName.Text = camera.LongName;
            tbCamerasXResolution.Text = camera.MaxResolution.Width.ToString();
            tbCamerasYResolution.Text = camera.MaxResolution.Height.ToString();

            foreach (var item in camera.ColorSpaces)
                lbCamerasColorSpaces.Items.Add(item.Name);
        }

        private void ClearCameraData()
        {
            tbCamerasId.Text = "(new)";
            tbCamerasShortName.Text = "";
            tbCamerasLongName.Text = "";
            tbCamerasXResolution.Text = "";
            tbCamerasYResolution.Text = "";
            lbCamerasColorSpaces.Items.Clear();
        }

        // Catalogues
        private void tbCataloguesShortName_Validating(object sender, CancelEventArgs e)
        {
            if (tbCataloguesShortName.Text.Length == 0)
                errorProvider.SetError(tbCataloguesShortName, "Must provide a value.");
            else
                errorProvider.SetError(tbCataloguesShortName, "");
        }

        // Color Spaces
        private void btnColorSpacesSave_Click(object sender, EventArgs e)
        {
            // Validations:

            if (tbColorSpaceName.Text.Length == 0)
            {
                errorProvider.SetError(tbColorSpaceName, "Must provide a value.");
                return;
            }
            else
                errorProvider.SetError(tbColorSpaceName, "");

            if (!rbColorSpaceMultiChannelNo.Checked && !rbColorSpaceMultiChannelYes.Checked)
            {
                errorProvider.SetError(rbColorSpaceMultiChannelNo, "Must select a value.");
                return;
            }
            else errorProvider.SetError(rbColorSpaceMultiChannelNo, "");
            
            // Save new/changes:

            int id = 0;
            string name = tbColorSpaceName.Text;
            byte bitsPerChannel = Convert.ToByte(tbColorSpaceBitsPerChannel.Value);
            bool isMultiChannel = rbColorSpaceMultiChannelYes.Checked;

            if (emColorSpaces == EditingModes.Add)
                id = 0;
            else
                id = currentColorSpace.Id;

            ColorSpace candidate = new ColorSpace(id, name, bitsPerChannel, isMultiChannel);

            if (emColorSpaces == EditingModes.Add)
                id = Operations.AddColorSpace(candidate);
            else
                Operations.EditColorSpace(id, candidate);

            // Reload data:

            currentColorSpace = Operations.GetColorSpaces(new List<int>() { id })[0];
            LoadColorSpaceData(currentColorSpace);
        }

        private void btnColorSpacesNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emColorSpaces, ref lblColorSpaces, "Color Spaces");

            ClearColorSpaceData();
        }

        private void lbColorSpaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emColorSpaces, ref lblColorSpaces, "Color Spaces");

            currentColorSpace = listColorSpaces[lbColorSpaces.SelectedIndex];
            LoadColorSpaceData(currentColorSpace);
        }

        private void LoadColorSpaceData(ColorSpace colorSpace)
        {
            tbColorSpaceId.Text = colorSpace.Id.ToString();
            tbColorSpaceName.Text = colorSpace.Name;
            tbColorSpaceBitsPerChannel.Value = colorSpace.BitsPerChannel;
        }

        private void ClearColorSpaceData()
        {
            tbColorSpaceId.Text = "(new)";
            tbColorSpaceName.Text = "";
            tbColorSpaceBitsPerChannel.Value = 1;
        }

        // File Formats
        private void tbFileFormatShortName_Validating(object sender, CancelEventArgs e)
        {
            if (tbFileFormatShortName.Text.Length == 0)
                errorProvider.SetError(tbFileFormatShortName, "Must provide a value.");
            else 
                errorProvider.SetError(tbFileFormatShortName, "");
        }

        // Optics
        private void cbOpticType_Validating(object sender, CancelEventArgs e)
        {
            if (cbOpticType.SelectedIndex == -1)
                errorProvider.SetError(cbOpticType, "Must provide a value.");
            else
                errorProvider.SetError(cbOpticType, "");
        }

        private void tbOpticValue_Validating(object sender, CancelEventArgs e)
        {
            if (tbOpticValue.Text.Length == -1)
            {
                errorProvider.SetError(tbOpticValue, "Must provide a value.");
                return;
            }

            if (!float.TryParse(tbOpticValue.Text, out _))
            {
                errorProvider.SetError(tbOpticValue, "Invalid format.");
                return;
            }

            errorProvider.SetError(tbOpticValue, "");
        }
    }
}
