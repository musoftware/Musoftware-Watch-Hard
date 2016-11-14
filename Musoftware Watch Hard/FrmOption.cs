using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;
using System.IO;
[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
    All = "HKEY_CURRENT_USER")]

namespace Musoftware_Watch_Hard
{
    public partial class FrmOption : Form
    {
        public FrmOption()
        {
            InitializeComponent();
        }
     
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey MyKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\run", true);
            string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WatchHDD\WatchHDD.exe";
            string Dirname = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WatchHDD";

            string PDirname = Path.GetDirectoryName(Application.ExecutablePath);

       
            if (checkBox1.Checked == false)
            {
                try { MyKey.DeleteValue("WatchHDD"); }
                catch (System.Exception) { }

                try { File.Delete(filename); }
                catch (System.Exception) { }

                return;
            }


            
            System.Security.Permissions.FileIOPermission permission = new
            System.Security.Permissions.FileIOPermission(
            System.Security.Permissions.FileIOPermissionAccess.Write,
            filename);

            try { File.Delete(filename); }
            catch (System.Exception) { }
            try
            { Directory.CreateDirectory(Dirname); }
            catch (System.Exception) { }
            try
            { File.Copy(Application.ExecutablePath, filename); }
            catch (System.Exception) { }
            try
            { File.Copy(PDirname + @"\FastColoredTextBox.dll", Dirname + @"\FastColoredTextBox.dll"); }
            catch (System.Exception) { }
            try
            { File.Copy(PDirname + @"\Tracker.dll", Dirname + @"\Tracker.dll"); }
            catch (System.Exception) { }


            MyKey.SetValue("WatchHDD", filename + " /hide");

        }

        private void FrmOption_Load(object sender, EventArgs e)
        {
            RegistryKey MyKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\run");
            try
            {
                if (!(MyKey.GetValue("WatchHDD") == null))
                    checkBox1.Checked = true;

            }
            catch (System.Exception)
            {
            }
        }
    }
}
