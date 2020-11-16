using AstroDAM.Controllers;
using AstroDAM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroDAM
{
    public static class Utilities
    {
        /// <summary>
        /// Checks whether a specific object is in use in the collections table before deletion.
        /// </summary>
        /// <param name="Type">Object Type</param>
        /// <param name="Id">Object id</param>
        /// <returns></returns>
        public static bool IsDeletionCollision(frmManager.ManagerTabs Type, int Id)
        {
            SqlConnection con = DbManager.GetConnection();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT [Id] FROM [tblCollections] ";

            switch (Type)
            {
                case frmManager.ManagerTabs.Cameras:
                    cmd.CommandText = "WHERE [Camera] = @Id";
                    break;
                case frmManager.ManagerTabs.Catalogues:
                    cmd.CommandText = "WHERE [CatalogueId] = @Id";

                    break;
                case frmManager.ManagerTabs.ColorSpaces:
                    cmd.CommandText = "WHERE [ColorSpace] = @Id";

                    break;
                case frmManager.ManagerTabs.FileFormats:
                    cmd.CommandText = "WHERE [FileFormat] = @Id";

                    break;
                case frmManager.ManagerTabs.Optics:
                    cmd.CommandText = "WHERE [Optics] LIKE '%@Id%'";
                    
                    break;
                case frmManager.ManagerTabs.Photographers:
                    cmd.CommandText = "WHERE [Photographer] = @Id";

                    break;
                case frmManager.ManagerTabs.Scopes:
                    cmd.CommandText = "WHERE [Scope] = @Id";

                    break;
                case frmManager.ManagerTabs.Sites:
                    cmd.CommandText = "WHERE [Site] = @Id";

                    break;
                default:
                    break;
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            return cmd.ExecuteReader().HasRows;
        }

        public static Size StringToResolution(string str)
        {
            List<int> xyData = StringToIntList(str);

            return new Size(xyData[0], xyData[1]);
        }

        public static string ResolutionToString(Size size)
        {
            return IntListToString(new List<int>() { size.Width, size.Height });
        }

        public static List<int> StringToIntList(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new List<int>();

            string[] nums = str.Split(';');
            List<int> numList = new List<int>(nums.Length);

            foreach (string num in nums)
                numList.Add(int.Parse(num));

            return numList;
        }

        public static string IntListToString(List<int> list)
        {
            string res = "";

            foreach (int i in list)
                res += i.ToString() + ";";

            return res.TrimEnd(';');
        }

     
        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static void PopulateTreeView(ref TreeView treeView, bool isAscending, string format = "")
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            treeView.Nodes.Clear();

            // returns dates, ids and object titles of collections
            cmd.CommandText = "SELECT [Id],CAST(FLOOR(CAST([CaptureDateTime] as FLOAT)) as DateTime) FROM tblCollections ORDER BY [CaptureDateTime] " + (isAscending ? "ASC" : "DESC");

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // for every collection, add to a new or existing group - by date.
                int id = reader.GetInt32(0);
                Collection col = CollectionController.GetCollections(new List<int>() { id })[0];

                string folderKey;

                if (Properties.Preferences.Default.NodeGrouping == 0)
                    folderKey = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                else if (Properties.Preferences.Default.NodeGrouping == 1)
                    folderKey = col.Catalogue.LongName;
                else
                    folderKey = col.ObjectTitle;

                string posibleKey = "f" + folderKey; // f is for folder.
                if (!treeView.Nodes.ContainsKey(posibleKey))
                    treeView.Nodes.Add(posibleKey, posibleKey.Substring(1));

                string namingFormat = string.IsNullOrEmpty(format) ? Properties.Preferences.Default.TreeNodeFormat : format;

                foreach (Match match in Regex.Matches(namingFormat, @"\{([^}]+)\}"))
                {
                    // parse the match:
                    string matchVal = match.Value.Replace("{", "").Replace("}", "");

                    string command, parameter = "";
                    if (matchVal.Contains("|"))
                    {
                        command = matchVal.Split('|')[0];
                        parameter = matchVal.Split('|')[1];
                    }
                    else
                        command = matchVal;

                    matchVal = "{" + matchVal + "}";

                    switch (command)
                    {
                        case "dt":
                            namingFormat = namingFormat.Replace(matchVal, col.CaptureDateTime.ToString(parameter));

                            break;

                        case "o":
                            if (parameter == "id")
                                namingFormat = namingFormat.Replace(matchVal, col.Object.ToString());

                            if (parameter == "n")
                                namingFormat = namingFormat.Replace(matchVal, col.ObjectTitle);

                            break;

                        case "c":
                            if (parameter == "id")
                                namingFormat = namingFormat.Replace(matchVal, col.Catalogue.Id.ToString());
                            
                            if (parameter == "sn")
                                namingFormat = namingFormat.Replace(matchVal, col.Catalogue.ShortName);
                            
                            if (parameter == "ln")
                                namingFormat = namingFormat.Replace(matchVal, col.Catalogue.LongName);

                            break;

                        default:
                            break;
                    }
                }
                

                //treeView.Nodes[posibleKey].Nodes.Add("i" + reader.GetString(0), // key. i for item.
                //    reader.GetDateTime(3).ToString("hh:mm:ss") + " - " + reader.GetString(2));

                treeView.Nodes[posibleKey].Nodes.Add("i" + id.ToString(), // key. i for item.
                    namingFormat);

                treeView.ExpandAll();
            }
        }
    }
}
