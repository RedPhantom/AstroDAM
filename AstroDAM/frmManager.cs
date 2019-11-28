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
            PrepareDataLists();

            lblStatus.Text = "Imported assets.";
        }

        void PrepareDataLists()
        {
            // Populate all hard-coded combo-box values.

            cbOpticType.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(Optic.OpticTypes)))
            {
                cbOpticType.Items.Add(item);
            }

            cbScopeMountType.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(Scope.MountTypes)))
            {
                cbScopeMountType.Items.Add(item);
            }

            cbSiteLongtitudeType.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(Site.LongtitudeTypes)))
            {
                cbSiteLongtitudeType.Items.Add(item);
            }

            cbSiteLatitudeType.Items.Clear();
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
            lbCameras.Items.Clear();
            foreach (var item in listCameras)
                lbCameras.Items.Add(item.Id + " " + item.LongName + " " + (item.MaxResolution.Width * item.MaxResolution.Height / 1000000).ToString() + "MP");

            lbCatalogues.Items.Clear();
            foreach (var item in listCatalogues)
                lbCatalogues.Items.Add(item.Id + " " + item.LongName);

            lbColorSpaces.Items.Clear();
            foreach (var item in listColorSpaces)
                lbColorSpaces.Items.Add(item.Id + " " + item.Name);

            lbFileFormats.Items.Clear();
            foreach (var item in listFileFormats)
                lbFileFormats.Items.Add(item.Id + " " + item.LongName);

            lbOptics.Items.Clear();
            foreach (var item in listOptics)
                lbOptics.Items.Add(item.Id + " " + item.Value + " " + item.OpticType.ToString());

            lbPhotographers.Items.Clear();
            foreach (var item in listPhotographers)
                lbPhotographers.Items.Add(item.Id + " " + item.LastName + ", " + item.FirstName);

            lbScopes.Items.Clear();
            foreach (var item in listScopes)
                lbScopes.Items.Add(item.Id + " " + item.Manufacturer + " " + item.Name);

            lbSites.Items.Clear();
            foreach (var item in listSites)
                lbSites.Items.Add(item.Id + " " + item.Name);

            // Populate specific database fields for every tab.

            cbCamerasColorSpaces.Items.Clear();
            foreach (var item in listColorSpaces)
                cbCamerasColorSpaces.Items.Add(item.Name);
        }

        private void UpdateEditingMode(EditingModes em, ref EditingModes editingModeVariable, ref Label tabLabel, string tabLabelText)
        {
            switch (em)
            {
                case EditingModes.Add:
                    editingModeVariable = EditingModes.Add;
                    tabLabel.Text = tabLabelText + " (adding)";
                    lblStatus.Text = "Switched to adding mode.";

                    break;
                case EditingModes.Edit:
                    editingModeVariable = EditingModes.Edit;
                    tabLabel.Text = tabLabelText + " (editing)";
                    lblStatus.Text = "Switched to editing mode.";

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
            lblStatus.Text = "Issuing camera saving command... ";
            currentCamera = Operations.GetCameras(new List<int>() { id })[0];
            LoadCameraData(currentCamera);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
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

        private void btnCamerasAddColorSpace_Click(object sender, EventArgs e)
        {
            int index = cbCamerasColorSpaces.SelectedIndex;
            if (index != -1)
            {
                ColorSpace candidate = listColorSpaces[index];
                
                if (!currentCamera.ColorSpaces.Contains(candidate))
                {
                    lbCamerasColorSpaces.Items.Add(candidate.Name);
                    currentCamera.ColorSpaces.Add(candidate);
                }
            }
        }

        private void btnCamerasRemoveColorSpace_Click(object sender, EventArgs e)
        {
            int index = lbCamerasColorSpaces.SelectedIndex;
            if (index != -1)
            {
                ColorSpace candidate = listColorSpaces[index];

                lbCamerasColorSpaces.Items.Remove(candidate.Name);
                currentCamera.ColorSpaces.Remove(candidate);
            }
        }
        
        private void LoadCameraData(Camera camera)
        {
            tbCamerasId.Text = camera.Id.ToString();
            tbCamerasShortName.Text = camera.ShortName;
            tbCamerasLongName.Text = camera.LongName;
            tbCamerasXResolution.Text = camera.MaxResolution.Width.ToString();
            tbCamerasYResolution.Text = camera.MaxResolution.Height.ToString();

            lbCamerasColorSpaces.Items.Clear();

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
        private void btnCataloguesSave_Click(object sender, EventArgs e)
        {
            // Valiations:

            if (tbCataloguesShortName.Text.Length == 0)
                errorProvider.SetError(tbCataloguesShortName, "Must provide a value.");
            else
                errorProvider.SetError(tbCataloguesShortName, "");

            // Save new/changes:

            int id = 0;
            string shortName = tbCataloguesShortName.Text;
            string longName = tbCataloguesLongName.Text;

            if (emCatalogues == EditingModes.Add)
                id = 0;
            else
                id = currentCatalogue.Id;

            Catalogue candidate = new Catalogue(id, shortName, longName);

            if (emCameras == EditingModes.Add)

                id = Operations.AddCatalogue(candidate);
            else
                Operations.EditCatalogue(id, candidate);

            // Reload data:

            lblStatus.Text = "Issuing catalogue saving command... ";
            currentCatalogue = Operations.GetCatalogues(new List<int>() { id })[0];
            LoadCatalogueData(currentCatalogue);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
        }

        private void btnCataloguesNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emCatalogues, ref lblCatalogues, "Catalogues");

            ClearCatalogueData();
        }

        private void lbCatalogues_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emCatalogues, ref lblCatalogues, "Catalogues");

            currentCatalogue = listCatalogues[lbCatalogues.SelectedIndex];
            LoadCatalogueData(currentCatalogue);
        }

        private void LoadCatalogueData(Catalogue catalogue)
        {
            tbCataloguesId.Text = catalogue.Id.ToString();
            tbCataloguesShortName.Text = catalogue.ShortName;
            tbCataloguesLongName.Text = catalogue.LongName;
        }

        private void ClearCatalogueData()
        {
            tbCataloguesId.Text = "(new)";
            tbCataloguesShortName.Text = "";
            tbCataloguesLongName.Text = "";
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

            lblStatus.Text = "Issuing color space saving command... ";
            currentColorSpace = Operations.GetColorSpaces(new List<int>() { id })[0];
            LoadColorSpaceData(currentColorSpace);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
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
            rbColorSpaceMultiChannelYes.Checked = colorSpace.IsMultiChannel;
            rbColorSpaceMultiChannelNo.Checked = !colorSpace.IsMultiChannel;
        }

        private void ClearColorSpaceData()
        {
            tbColorSpaceId.Text = "(new)";
            tbColorSpaceName.Text = "";
            tbColorSpaceBitsPerChannel.Value = 1;
        }

        // File Formats

        private void btnFileFormatsSave_Click(object sender, EventArgs e)
        {
            // Valiations:

            if (tbFileFormatShortName.Text.Length == 0)
                errorProvider.SetError(tbFileFormatShortName, "Must provide a value.");
            else
                errorProvider.SetError(tbFileFormatShortName, "");

            // Save new/changes:

            int id = 0;
            string shortName = tbFileFormatShortName.Text;
            string longName = tbFileFormatLongName.Text;

            if (emFileFormats == EditingModes.Add)
                id = 0;
            else
                id = currentFileFormat.Id;

            FileFormat candidate = new FileFormat(id, shortName, longName);

            if (emFileFormats == EditingModes.Add)

                id = Operations.AddFileFormat(candidate);
            else
                Operations.EditFileFormat(id, candidate);

            // Reload data:

            lblStatus.Text = "Issuing file formats saving command... ";
            currentFileFormat = Operations.GetFileFormats(new List<int>() { id })[0];
            LoadFileFormatData(currentFileFormat);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
        }

        private void btnFileFormatsNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emFileFormats, ref lblFileFormats, "File Formats");

            ClearFileFormatData();
        }

        private void lbFileFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emFileFormats, ref lblFileFormats, "File Formats");

            currentFileFormat = listFileFormats[lbFileFormats.SelectedIndex];
            LoadFileFormatData(currentFileFormat);
        }

        private void LoadFileFormatData(FileFormat fileFormat)
        {
            tbFileFormatId.Text = fileFormat.Id.ToString();
            tbFileFormatShortName.Text = fileFormat.ShortName;
            tbFileFormatLongName.Text = fileFormat.LongName;
        }

        private void ClearFileFormatData()
        {
            tbFileFormatId.Text = "(new)";
            tbFileFormatShortName.Text = "";
            tbFileFormatLongName.Text = "";
        }

        // Optics
        private void btnOpticsSave_Click(object sender, EventArgs e)
        {
            // Valiations:

            if (cbOpticType.SelectedIndex == -1)
                errorProvider.SetError(cbOpticType, "Must provide a value.");
            else
                errorProvider.SetError(cbOpticType, "");

            if (tbOpticValue.Text.Length == -1)
            {
                errorProvider.SetError(tbOpticValue, "Must provide a value.");
                return;
            } else
                errorProvider.SetError(tbOpticValue, "");


            if (!float.TryParse(tbOpticValue.Text, out _))
            {
                errorProvider.SetError(tbOpticValue, "Invalid format.");
                return;
            }
            else
                errorProvider.SetError(tbOpticValue, "");

            // Save new/changes:

            int id = 0;
            Optic.OpticTypes type = (Optic.OpticTypes)cbOpticType.SelectedIndex;
            double value = double.Parse(tbOpticValue.Text);

            if (emOptics == EditingModes.Add)
                id = 0;
            else
                id = currentOptic.Id;

            Optic candidate = new Optic(id, type, value);

            if (emOptics == EditingModes.Add)

                id = Operations.AddOptics(candidate);
            else
                Operations.EditOptics(id, candidate);

            // Reload data:

            lblStatus.Text = "Issuing optics saving command... ";
            currentOptic = Operations.GetOptics(new List<int>() { id })[0];
            LoadOpticData(currentOptic);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
        }

        private void btnOpticsNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emOptics, ref lblOptics, "Optics");

            ClearOpticData();
        }

        private void lbOptics_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emOptics, ref lblOptics, "Optics");

            currentOptic = listOptics[lbOptics.SelectedIndex];
            LoadOpticData(currentOptic);
        }

        private void LoadOpticData(Optic Optic)
        {
            tbOpticId.Text = Optic.Id.ToString();
            cbOpticType.SelectedIndex = (int)Optic.OpticType;
            tbOpticValue.Text = Optic.Value.ToString();
        }

        private void ClearOpticData()
        {
            tbOpticId.Text = "(new)";
            cbOpticType.SelectedIndex = -1;
            tbOpticValue.Text = "";
        }

        // Photographers

        private void btnPhotographersSave_Click(object sender, EventArgs e)
        {
            // Valiations:

            if (tbPhotographerFirstName.Text.Length == 0)
                errorProvider.SetError(tbPhotographerFirstName, "Must provide a value.");
            else
                errorProvider.SetError(tbPhotographerFirstName, "");

            // Save new/changes:

            int id = 0;
            string FirstName = tbPhotographerFirstName.Text;
            string LastName = tbPhotographerLastName.Text;

            if (emPhotographers == EditingModes.Add)
                id = 0;
            else
                id = currentPhotographer.Id;

            Photographer candidate = new Photographer(id, FirstName, LastName);

            if (emPhotographers == EditingModes.Add)

                id = Operations.AddPhotographer(candidate);
            else
                Operations.EditPhotographer(id, candidate);

            // Reload data:

            lblStatus.Text = "Issuing photographer saving command... ";
            currentPhotographer = Operations.GetPhotographers(new List<int>() { id })[0];
            LoadPhotographerData(currentPhotographer);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
        }

        private void btnPhotographersNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emPhotographers, ref lblPhotographers, "Photographers");

            ClearPhotographerData();
        }

        private void lbPhotographers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emPhotographers, ref lblPhotographers, "Photographers");

            currentPhotographer = listPhotographers[lbPhotographers.SelectedIndex];
            LoadPhotographerData(currentPhotographer);
        }

        private void LoadPhotographerData(Photographer Photographer)
        {
            tbPhotographerId.Text = Photographer.Id.ToString();
            tbPhotographerFirstName.Text = Photographer.FirstName;
            tbPhotographerLastName.Text = Photographer.LastName;
        }

        private void ClearPhotographerData()
        {
            tbPhotographerId.Text = "(new)";
            tbPhotographerFirstName.Text = "";
            tbPhotographerLastName.Text = "";
        }

        // Scopes
        private void btnScopesSave_Click(object sender, EventArgs e)
        {
            // Valiations:
            // TODO check for valid float values
            if (tbScopeManufacturer.Text.Length == 0)
                errorProvider.SetError(tbScopeManufacturer, "Must provide a value.");
            else
                errorProvider.SetError(tbScopeManufacturer, "");

            if (tbScopeName.Text.Length == 0)
                errorProvider.SetError(tbScopeName, "Must provide a value.");
            else
                errorProvider.SetError(tbScopeName, "");

            if (tbScopeAperture.Text.Length == 0)
                errorProvider.SetError(tbScopeAperture, "Must provide a value.");
            else
                errorProvider.SetError(tbScopeAperture, "");

            if (tbScopeFocalLength.Text.Length == 0)
                errorProvider.SetError(tbScopeFocalLength, "Must provide a value.");
            else
                errorProvider.SetError(tbScopeFocalLength, "");

            if (tbScopeCentralObstructionDiameter.Text.Length == 0)
                errorProvider.SetError(tbScopeCentralObstructionDiameter, "Must provide a value.");
            else
                errorProvider.SetError(tbScopeCentralObstructionDiameter, "");

            if (!rbScopeRoboticYes.Checked && !rbScopeRoboticNo.Checked)
                errorProvider.SetError(rbScopeRoboticNo, "Must provide a value.");
            else
                errorProvider.SetError(rbScopeRoboticNo, "");

            if (cbScopeMountType.SelectedIndex == -1)
                errorProvider.SetError(cbScopeMountType, "Must provide a value.");
            else
                errorProvider.SetError(cbScopeMountType, "");

            // Save new/changes:

            int id = 0;
            string manufacturer = tbScopeManufacturer.Text;
            string name = tbScopeName.Text;
            float aperture = float.Parse(tbScopeAperture.Text);
            float focalLength = float.Parse(tbScopeFocalLength.Text);
            float centralObstructionDiameter = float.Parse(tbScopeCentralObstructionDiameter.Text);
            bool robotic = rbScopeRoboticYes.Checked;
            Scope.MountTypes mountType = (Scope.MountTypes)cbScopeMountType.SelectedIndex;

            if (emScopes == EditingModes.Add)
                id = 0;
            else
                id = currentScope.Id;

            Scope candidate = new Scope(id, manufacturer,name, aperture, focalLength, centralObstructionDiameter, robotic, mountType);

            if (emScopes == EditingModes.Add)

                id = Operations.AddScope(candidate);
            else
                Operations.EditScope(id, candidate);

            // Reload data:

            lblStatus.Text = "Issuing scopes saving command... ";
            currentScope = Operations.GetScopes(new List<int>() { id })[0];
            LoadScopeData(currentScope);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
        }

        private void btnScopesNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emScopes, ref lblScopes, "Scopes");

            ClearScopeData();
        }

        private void lbScopes_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emScopes, ref lblScopes, "Scopes");

            if (lbScopes.SelectedIndex != -1)
            {
                currentScope = listScopes[lbScopes.SelectedIndex];
                LoadScopeData(currentScope);
            }
        }

        private void LoadScopeData(Scope scope)
        {
            tbScopeId.Text = scope.Id.ToString();
            tbScopeManufacturer.Text = scope.Manufacturer;
            tbScopeName.Text = scope.Name;
            tbScopeAperture.Text = scope.Aperture.ToString();
            tbScopeFocalLength.Text = scope.FocalLength.ToString();
            tbScopeCentralObstructionDiameter.Text = scope.CentralObstructionDiameter.ToString();
            rbScopeRoboticYes.Checked = scope.Robotic;
            rbScopeRoboticNo.Checked = !scope.Robotic;
            cbScopeMountType.SelectedIndex = (int)scope.MountType;
        }

        private void ClearScopeData()
        {
            tbScopeId.Text = "(new)";
            tbScopeManufacturer.Text = "";
            tbScopeName.Text = "";
            tbScopeAperture.Text = "";
            tbScopeFocalLength.Text = "";
            tbScopeCentralObstructionDiameter.Text = "";
            rbColorSpaceMultiChannelYes.Checked = false;
            rbColorSpaceMultiChannelNo.Checked = false;
            cbScopeMountType.SelectedIndex = -1;
        }

        // Sites

        private void btnSitesSave_Click(object sender, EventArgs e)
        {
            // Valiations:

            if (tbSiteName.Text.Length == 0)
                errorProvider.SetError(tbSiteName, "Must provide a value.");
            else
                errorProvider.SetError(tbSiteName, "");

            if (tbSiteLongtitude.Text.Length == 0)
                errorProvider.SetError(tbSiteLongtitude, "Must provide a value.");
            else
                errorProvider.SetError(tbSiteLongtitude, "");

            if (tbSiteLatitude.Text.Length == 0)
                errorProvider.SetError(tbSiteLatitude, "Must provide a value.");
            else
                errorProvider.SetError(tbSiteLatitude, "");

            if (cbSiteLongtitudeType.SelectedIndex == -1)
                errorProvider.SetError(cbSiteLongtitudeType, "Must provide a value.");
            else
                errorProvider.SetError(cbSiteLongtitudeType, "");

            if (cbSiteLatitudeType.SelectedIndex == -1)
                errorProvider.SetError(cbSiteLatitudeType, "Must provide a value.");
            else
                errorProvider.SetError(cbSiteLatitudeType, "");

            // Save new/changes:

            int id = 0;
            string name = tbSiteName.Text;
            float longtitude = float.Parse(tbSiteLongtitude.Text);
            float latitude = float.Parse(tbSiteLatitude.Text);
            Site.LongtitudeTypes longtitudeType = (Site.LongtitudeTypes)cbSiteLongtitudeType.SelectedIndex;
            Site.LatitudeTypes latitudeType = (Site.LatitudeTypes)cbSiteLatitudeType.SelectedIndex;

            if (emSites == EditingModes.Add)
                id = 0;
            else
                id = currentSite.Id;

            Site candidate = new Site(id, name, longtitude, longtitudeType, latitude, latitudeType); ;

            if (emSites == EditingModes.Add)

                id = Operations.AddSite(candidate);
            else
                Operations.EditSite(id, candidate);

            // Reload data:

            lblStatus.Text = "Issuing sites saving command... ";
            currentSite = Operations.GetSites(new List<int>() { id })[0];
            LoadSiteData(currentSite);
            PrepareDataLists();
            lblStatus.Text += "Complete.";
        }

        private void btnSitesNew_Click(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Add, ref emSites, ref lblSites, "Sites");

            ClearSiteData();
        }

        private void lbSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditingMode(EditingModes.Edit, ref emSites, ref lblSites, "Sites");

            currentSite = listSites[lbSites.SelectedIndex];
            LoadSiteData(currentSite);
        }

        private void LoadSiteData(Site Site)
        {
            tbSiteId.Text = Site.Id.ToString();
            tbSiteName.Text = Site.Name;
            tbSiteLongtitude.Text = Site.Longtitude.ToString();
            cbSiteLongtitudeType.SelectedIndex = (int)Site.LongtitudeType;
            tbSiteLatitude.Text = Site.Latitude.ToString();
            cbSiteLatitudeType.SelectedIndex = (int)Site.LatitudeType;
        }

        private void ClearSiteData()
        {
            tbSiteId.Text = "(new)";
            tbSiteLongtitude.Text = "";
            cbSiteLongtitudeType.SelectedIndex = -1;
            tbSiteLatitude.Text = "";
            cbSiteLatitudeType.SelectedIndex = -1;
        }
    }
}
