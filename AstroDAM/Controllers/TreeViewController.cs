﻿using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    public static class TreeViewController
    {
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
