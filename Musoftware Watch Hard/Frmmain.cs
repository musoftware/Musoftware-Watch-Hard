using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using Musoftware_Watch_Hard.Properties;
namespace Musoftware_Watch_Hard
{
    public partial class Frmmain : Form
    {
        public Frmmain()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Double Reading = Math.Round(Convert.ToDouble(performanceCounter1.NextValue().ToString()));
            Double Writing = Math.Round(Convert.ToDouble(performanceCounter2.NextValue().ToString()));
            if (Reading == 0) notifyIcon1.Icon = Resources.Green; else notifyIcon1.Icon = Resources._Green;
            if (Writing == 0) notifyIcon2.Icon = Resources.Red; else notifyIcon2.Icon = Resources._Red;

            tracker1.Value = Convert.ToInt32(Math.Round(Reading / (30 * 1024 * 1024) * 100));
            tracker2.Value = Convert.ToInt32(Math.Round(Writing / (30 * 1024 * 1024) * 100));     
        }

        private void Frmmain_Load(object sender, EventArgs e)
        {

            checkinstance();
            foreach (System.IO.DriveInfo i in System.IO.DriveInfo.GetDrives())
            {
                comboBox1.Items.Add(i.RootDirectory);
            }
            comboBox1.SelectedIndex = 0;
            
        }

        public void checkinstance()
        {
            Process[] thisnameprocesslist;
            string modulename, processname;
            Process p = Process.GetCurrentProcess();
            modulename = p.MainModule.ModuleName.ToString();
            processname = System.IO.Path.GetFileNameWithoutExtension(modulename);
            thisnameprocesslist = Process.GetProcessesByName(processname);
            if (thisnameprocesslist.Length > 1)
            {
                MessageBox.Show("Instance of this application is already running.");
                Application.Exit();
            }
        }
        private void Re(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            { this.Visible = false; this.ShowInTaskbar = false; }

            if (this.WindowState == FormWindowState.Normal)
            { this.Visible = true; this.ShowInTaskbar = true; }
        }

        private void ShowAgain(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            FrmAbout D = new FrmAbout();
            D.ShowDialog();
        }

        TextStyle chngStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        TextStyle nowStyle = new TextStyle(Brushes.Chocolate, null, FontStyle.Bold);
        TextStyle crtStyle = new TextStyle(Brushes.BurlyWood, null, FontStyle.Regular);
        TextStyle delStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);


        private void FCja(object sender, System.IO.FileSystemEventArgs e)
        {
            if (!fctb.Text.Contains(" Changed\r\n  " + e.FullPath))
            {
                Log(" Changed\r\n  " + e.FullPath
                    + "\r\n", chngStyle);
                xchangedx.Add(e.FullPath);
            }

            if (checkBox1.Checked == true)
                fctb.GoEnd();

        }

        List<string> xchangedx=new List<string>();


        private void FCra(object sender, System.IO.FileSystemEventArgs e)
        {
            if (!fctb.Text.Contains(" Created\r\n   " + e.FullPath))
                Log(" Created\r\n   " + e.FullPath 
                    +"\r\n", crtStyle);
            xchangedx.Add(e.FullPath);
            if (checkBox1.Checked == true)
                fctb.GoEnd();
        }
        int oldsel;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                oldsel = comboBox1.SelectedIndex;
                fctb.Text = "";
                xchangedx.Clear();
                fileSystemWatcher1.Path = comboBox1.Text;
            }
            catch (Exception)
            {
                comboBox1.SelectedIndex = oldsel;
            }
            
 
            
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            FrmOption D = new FrmOption();
            D.ShowDialog();
        }

        private void FDia(object sender, System.IO.FileSystemEventArgs e)
        {
            if (!fctb.Text.Contains(" Deleted\r\n   " + e.FullPath))
                Log(" Deleted\r\n   " + e.FullPath
                    + "\r\n", delStyle);
            if (checkBox1.Checked == true)
                fctb.GoEnd();
        }

        private void Log(string text, Style style)
        {
            //some stuffs for best performance
            fctb.BeginUpdate();
            fctb.Selection.BeginUpdate();
            //remember user selection
            var userSelection = fctb.Selection.Clone();
            //goto end of the text
            fctb.Selection.Start = fctb.LinesCount > 0 ? new Place(fctb[fctb.LinesCount - 1].Count, fctb.LinesCount - 1) : new Place(0, 0);
            //add text with predefined style
            fctb.InsertText(Convert.ToString(DateTime.Now), nowStyle);

            fctb.InsertText(text, style);
            //restore user selection
            if (!userSelection.IsEmpty || userSelection.Start.iLine < fctb.LinesCount - 2)
            {
                fctb.Selection.Start = userSelection.Start;
                fctb.Selection.End = userSelection.End;
            }
            else
                fctb.DoCaretVisible();//scroll to end of the text
            //
            fctb.Selection.EndUpdate();
            fctb.EndUpdate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Scrol(object sender, ScrollEventArgs e)
        {
            checkBox1.Checked = false;
        }

        private void CG(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(13))
            try
            {
                oldsel = comboBox1.SelectedIndex;
                fctb.Text = "";
                fileSystemWatcher1.Path = comboBox1.Text;
            }
            catch (Exception)
            {
                comboBox1.SelectedIndex = oldsel;
            }

        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Md= new FolderBrowserDialog();

            if (Md.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                foreach (string I in xchangedx)
                {
                 //   comboBox1.Text 
                    string Newpaths = System.IO.Path.GetDirectoryName(I).Replace(comboBox1.Text.TrimEnd('\\'), Md.SelectedPath.TrimEnd('\\'));

                    System.IO.Directory.CreateDirectory(
                       Newpaths);
                    try
                    {
                        System.IO.File.Copy(I, Newpaths.TrimEnd('\\') + "\\" + System.IO.Path.GetFileName(I));
                    }
                        catch
                    {

                    }
                  
                }

            }


        }

  

    }
}
