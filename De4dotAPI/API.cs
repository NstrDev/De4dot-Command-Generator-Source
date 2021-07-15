using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace De4dotAPI
{
    public class API
    {
        public static void Generate_Command(string dedotfile, string arguments, MetroTextBox outputlogs, bool open_cmd)
        {
            try
            {
                string filename = Path.GetFileName(dedotfile);
                string direc = Path.GetDirectoryName(dedotfile);
                string command = $"{Path.GetFileNameWithoutExtension(dedotfile)}{arguments}";
                outputlogs.Text = command;
                MessageBox.Show("Commands Generated", "De4dot API");
                if (open_cmd)
                {
                    Process p = new Process();
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WorkingDirectory = direc;
                    p.StartInfo.FileName = "cmd.exe";
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}