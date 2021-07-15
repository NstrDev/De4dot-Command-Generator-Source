using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System.IO;
using De4dotAPI;

namespace De4dot_GUI
{
    public partial class Mainform : MetroForm
    {
        public Mainform()
        {
            InitializeComponent();
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            dedottype.SelectedIndex = 0; stringmode.SelectedIndex = 0; dedotpath.Text = Environment.CurrentDirectory;
        }

        private string Getdedotpath()
        {
            string path = "";
            string ddotype = "";
            switch (dedottype.SelectedIndex)
            {
                case 0:
                    ddotype = "de4dot.exe";
                    break;

                case 1:
                    ddotype = "de4dot-x64.exe";
                    break;
            }
            path = $@"{dedotpath.Text}\{ddotype}";
            return path;
        }

        private string GetArguments()
        {
            if (!File.Exists(inputfile.Text))
            {
                MessageBox.Show("Invalid Input File", "De4dot API");
                return null;
            }

            string argument;

            if (deobmode.Checked)
            {
                #region Outputfile

                string outputfilename = "";
                if (dntrname.Checked)
                {
                    outputfilename = Path.GetFileName(inputfile.Text);
                }
                else
                {
                    outputfilename = $"{Path.GetFileNameWithoutExtension(inputfile.Text)}.de4dot{Path.GetExtension(inputfile.Text)}";
                }
                Directory.CreateDirectory($@"{Path.GetDirectoryName(inputfile.Text)}\de4dot");
                string outputfile = $@"{Path.GetDirectoryName(inputfile.Text)}\de4dot\{outputfilename}";

                #endregion Outputfile

                #region String Mode

                string strngdecrmode = "";
                switch (stringmode.SelectedIndex)
                {
                    case 0:
                        strngdecrmode = "";
                        break;

                    case 1:
                        strngdecrmode = " --strtyp static";
                        break;

                    case 2:
                        strngdecrmode = " --strtyp delegate";
                        break;

                    case 3:
                        strngdecrmode = " --strtyp emulate";
                        break;

                    case 4:
                        strngdecrmode = " --strtyp none";
                        break;
                }

                #endregion String Mode

                #region Options

                string optns = "";
                if (prstoken.Checked)
                {
                    optns += " --preserve-tokens";
                }
                if (keptpes.Checked)
                {
                    optns += " --keep-types";
                }

                #endregion Options

                #region Additional

                string adtnlopts = "";
                if (!String.IsNullOrEmpty(aditcmd.Text))
                {
                    adtnlopts = " " + aditcmd.Text;
                }

                #endregion Additional

                argument = $" -f \"{inputfile.Text}\" -o \"{outputfile}\"{strngdecrmode}{optns}{adtnlopts}";
            }
            else
            {
                argument = $" -d \"{inputfile.Text}\"";
            }
            return argument;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            //Deob
            if (!File.Exists(Getdedotpath()))
            {
                MessageBox.Show("Invalid De4dot File", "De4dot API");
                return;
            }
            try
            {
                if (GetArguments() != null)
                {
                    API.Generate_Command(Getdedotpath(), GetArguments(), logoutput, cmdopt.Checked);
                }
            } finally
            {
                cmdopt.Checked = false;
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Getdedotpath()))
            {
                MessageBox.Show("Invalid De4dot File", "De4dot API");
                return;
            }
            try
            {
                API.Generate_Command(Getdedotpath(), " -h", logoutput, cmdopt.Checked);
            } finally
            {
                cmdopt.Checked = false;
            }
        }

        private void srchdedot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            dedotpath.Text = folderBrowserDialog.SelectedPath;
        }

        private void srchinp_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "File (*.exe, *.dll)|*.exe;*.dll";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ReadOnlyChecked = false;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();
            inputfile.Text = openFileDialog.FileName;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(logoutput.Text);
        }
    }
}