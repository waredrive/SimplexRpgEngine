﻿using DarkUI.Docking;
using DarkUI.Forms;
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

namespace SimplexIde
{
    public partial class FormNew : DarkToolWindow
    {
        public FormNew()
        {
            InitializeComponent();

        }

        private void CreateDirectory(string newPath, string[] names)
        {
            foreach (var item in names)
            {
                Directory.CreateDirectory(newPath + item);
            }
        }
        private void NewFiles(string prePath, string newPath, string[] names){
            foreach (var item in names)
            {
                File.Copy(prePath + "Prefab.json", newPath + item);
            }
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            string path = Environment.CurrentDirectory;

            int i = path.LastIndexOf("\\");
            path = path.Substring(0, i);
            i = path.LastIndexOf("\\");
            path = path.Substring(0, i);
            i = path.LastIndexOf("\\");
            path = path.Substring(0, i);

            string prePath = path + "\\SimplexCore\\Prefabs\\";
            string newPath = path + "\\" + textName.Text;

            Directory.CreateDirectory(newPath);
            newPath += "\\";

            //Create .json files
            string[] names = new string[] { "ShadersDescriptor.json", "SoundsDescriptor.json", "SpritesDescriptor.json", "TilesetsDescriptor.json", "VideosDescriptor.json" };
            this.NewFiles(prePath, newPath, names);

            //Create new sproject
            File.Copy(prePath + "Prefab.sproject", newPath + textName.Text + ".sproject");

            //Create files and replace Prefabs
            File.Copy(prePath + "Global.cs", newPath + "Global.cs");
            string text = File.ReadAllText(newPath + "Global.cs");
            text = text.Replace("Prefab", textName.Text);
            File.WriteAllText(newPath + "Global.cs", text);

            File.Copy(prePath + "Prefab.projitems", newPath + textName.Text + ".projitems");
            text = File.ReadAllText(newPath + textName.Text + ".projitems");
            text = text.Replace("Prefab", textName.Text);
            File.WriteAllText(newPath + textName.Text + ".projitems", text);

            File.Copy(prePath + "Prefab.shproj", newPath + textName.Text + ".shproj");
            text = File.ReadAllText(newPath + textName.Text + ".shproj");
            text = text.Replace("Prefab", textName.Text);
            File.WriteAllText(newPath + textName.Text + ".shproj", text);

            //Create directories
            names = new string[] { "Content", "Objects", "Rooms", "Shaders" };
            this.CreateDirectory(newPath, names);

            newPath += "Content\\";
            names = new string[] { "Fonts", "Shaders", "Sounds", "Sprites", "Tilesets", "Videos" };
            this.CreateDirectory(newPath, names);

            //sln ref
            text = File.ReadAllText(path + "\\SimplexRpgEngine3.sln");
            i = text.IndexOf("Global");
            string rf = "Project(\"{" + Guid.NewGuid() + "}\") = \"" + textName.Text + "\", \"" + textName.Text + "\\" + textName.Text + ".shproj\", \"{" + Guid.NewGuid() + "}\"\r\n" + "EndProject";
            string s = text.Substring(0, i - 1) + "\r\n" + rf + "\r\n" + text.Substring(i, text.Length - i);
            File.WriteAllText(path + "\\SimplexRpgEngine3.sln", s);

            //csproj ref
            text = File.ReadAllText(path + "\\SimplexIde\\SimplexIde.csproj");
            i = text.IndexOf("<Import Project=\"..\\");
            rf = "<Import Project=\"..\\" + textName.Text + "\\" + textName.Text + ".projitems\" Label=\"Shared\" />";
            s = text.Substring(0, i - 1) + "\r\n" + rf + "\r\n" + text.Substring(i, text.Length - i);
            File.WriteAllText(path + "\\SimplexIde\\SimplexIde.csproj", s);

            Close();
        }
    }
}
