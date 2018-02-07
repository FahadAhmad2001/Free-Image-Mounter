using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
namespace ISO_Mounter
{
    public partial class Form2 : Form
    {
        String AutoUpdate;
        String CurrentVDate;
        String CurrentVersion;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\" + "version.txt"))
            {
                StreamReader CheckVersion;
                CheckVersion = new StreamReader(Application.StartupPath + "\\" + "version.txt");
                String FullFile = CheckVersion.ReadToEnd();
                CheckVersion.Close();
                String[] output1;
                Char Splitter = ':';
                output1 = FullFile.Split(Splitter);
                CurrentVersion = output1[0];
                CurrentVDate = output1[1];
                AutoUpdate = output1[2];
                AutoUpdate.TrimEnd('\r', '\n');
                if (AutoUpdate.Contains("true"))
                {
                    button1.Text = "Updates enabled";
                }
                else
                {
                    button1.Text = "Updates disabled";
                }
            }
        }
        public void UpdateProgram()
        {
            System.Net.WebClient DownloadProgram = new System.Net.WebClient();
            Uri address = new Uri("http://89.203.4.93:500/isomount/isomounterinstall.exe");
            DownloadProgram.DownloadProgressChanged += new DownloadProgressChangedEventHandler(UpdateProgress);
            DownloadProgram.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFinished);
            DownloadProgram.DownloadFileAsync(address, "isomounterinstall.exe");
        }
        public void UpdateProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            label1.Text = "Status: " + e.ProgressPercentage + " %";
            progressBar1.Value = e.ProgressPercentage;
        }
        public void DownloadFinished(object sender, AsyncCompletedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\" + "isomounterinstall.exe");
            Application.Exit();
            MessageBox.Show("Completed");
            Closeform();
        }
        public void Closeform()
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(AutoUpdate);
            if (AutoUpdate.Contains( "false"))
            {
                DialogResult ChangeUStatus = MessageBox.Show("Would you like to enable automatic updates?", "Auto Updates", MessageBoxButtons.YesNo);
                if (ChangeUStatus == DialogResult.Yes)
                {
                    String command5;
                    command5 = CurrentVersion + ":" + CurrentVDate + ":true";
                    StreamWriter UpdateVersionInfo;
                    UpdateVersionInfo = new StreamWriter(Application.StartupPath + "\\" + "version.txt");
                    UpdateVersionInfo.WriteLine(command5);
                    UpdateVersionInfo.Close();
                    button1.Text = "Updates enabled";
                    AutoUpdate = "true";
                    goto EndChange;

                }
            }
            if (AutoUpdate.Contains("true"))
            {
                DialogResult ChangeUStatus = MessageBox.Show("Would you like to disable automatic updates?", "Auto Updates", MessageBoxButtons.YesNo);
                if (ChangeUStatus == DialogResult.Yes)
                {
                    String command5;
                    command5 = CurrentVersion + ":" + CurrentVDate + ":false";
                    //command5.TrimEnd();
                    StreamWriter UpdateVersionInfo;
                    UpdateVersionInfo = new StreamWriter(Application.StartupPath + "\\" + "version.txt");
                    UpdateVersionInfo.WriteLine(command5);
                    UpdateVersionInfo.Close();
                    button1.Text = "Updates disabled";
                    AutoUpdate = "false";
                    goto EndChange;
                }
            }
            EndChange:;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\" + "version.txt"))
            {
                StreamReader CheckVersion;
                CheckVersion = new StreamReader(Application.StartupPath + "\\" + "version.txt");
                String FullFile = CheckVersion.ReadToEnd();

                CheckVersion.Close();

                String[] output1;
                Char Splitter = ':';
                output1 = FullFile.Split(Splitter);
                CurrentVersion = output1[0];
                CurrentVDate = output1[1];
                //AutoUpdate = output1[2];
                System.Net.NetworkInformation.Ping TestServer = new System.Net.NetworkInformation.Ping();
                    if (TestServer.Send("89.203.4.93", 300).Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        System.Net.WebClient GetLatestInfo = new System.Net.WebClient();
                        GetLatestInfo.DownloadFile("ftp://89.203.4.93/downloads/isomount/latestversion.txt", "latestversion.txt");
                        CheckVersion = new StreamReader(Application.StartupPath + "\\" + "latestversion.txt");
                        String LatestFile = CheckVersion.ReadToEnd();
                        CheckVersion.Close();
                        output1 = LatestFile.Split(Splitter);
                        String LatestVersion = output1[0];
                        if (LatestVersion == CurrentVersion)
                        {
                        MessageBox.Show("Latest version present");
                        }
                        else
                        {
                            DialogResult UpdateResponse = MessageBox.Show("New version, " + output1[0] + ", from " + output1[1] + " found. Update?", "Update available", MessageBoxButtons.YesNoCancel);
                            if (UpdateResponse == DialogResult.Yes)
                            {
                            UpdateProgram();
                            }
                        }
                    }
                else
                {
                    MessageBox.Show("Cannot contact update server");
                }
                
            }
        }
    }

}
