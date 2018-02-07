using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
//using System.Runtime.

namespace ISO_Mounter
{
    public partial class Form1 : Form
    {
        public string FileLocation;
        public string command1;
        public string command2;
        public StreamWriter editBAT;
        public string output1;
        public string error1;
        public String AutoUpdate;
        public String CurrentVersion;
        public String CurrentVDate;
        //public int unmountresponse;
        //public Process MountImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "Status: Selecting file";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                FileLocation = openFileDialog1.FileName;
                label2.Text = FileLocation;
                label3.Text = "Status: ";
            }
            else
            {
                label3.Text = "Status: ";
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            command1 = "Mount-DiskImage -ImagePath " + "'" + FileLocation + "'";
            //using (PowerShell PowerShellInstance = PowerShell.Create())
            //{
            //  PowerShellInstance.AddCommand("Mount-DiskImage");
            //PowerShellInstance.AddParameter();
            //PowerShellInstance.AddArgument(" -ImagePath " + "'" + FileLocation + "'");
            // PowerShellInstance.Invoke();

            //}
            editBAT = new StreamWriter(Application.StartupPath + "\\" + "MountISO.ps1");
            editBAT.WriteLine(command1);
            editBAT.Close();
            command2 = "powershell.exe -command " + command1;
            //MessageBox.Show(command2);
            System.Diagnostics.Process MountImage = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo MountImageInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            MountImageInfo.CreateNoWindow = true;
            MountImageInfo.UseShellExecute = false;
            MountImageInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            MountImageInfo.Arguments = "/c " + command2;
            //MountImageInfo.RedirectStandardError = true;
            //MountImageInfo.RedirectStandardOutput = true;
            MountImage.StartInfo = MountImageInfo;
            label3.Text = "Status: Mounting";
            MountImage.Start();
            //output1 = MountImage.StandardOutput.ReadToEnd();
            //error1 = MountImage.StandardError.ReadToEnd();
            //MessageBox.Show(output1);
            //MessageBox.Show(error1);
            MountImage.WaitForExit();
            MountImage.Close();
            dataGridView1.Rows.Add(FileLocation);
            label3.Text = "Status:";
            MessageBox.Show("Successfully mounted");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult unmountresponse  = MessageBox.Show("Do you really want to unmount this image","Confirm dismount",MessageBoxButtons.YesNo);
            if( unmountresponse == DialogResult.Yes){
                int CellIndex = dataGridView1.CurrentCell.RowIndex;
                String removeFile = dataGridView1.CurrentCell.Value.ToString();
                String command3 = "Dismount-DiskImage -ImagePath " + "'" + removeFile + "'";
                String command4 = "/c powershell.exe -command " + command3;
                System.Diagnostics.Process UnMountImage = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo UnMountImageInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                UnMountImageInfo.CreateNoWindow = true;
                UnMountImageInfo.UseShellExecute = false;
                UnMountImageInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                UnMountImageInfo.Arguments = command4;
                UnMountImage.StartInfo = UnMountImageInfo;
                label3.Text = "Status: Unmounting";
                UnMountImage.Start();
                UnMountImage.WaitForExit();
                UnMountImage.Close();
                dataGridView1.Rows.RemoveAt(CellIndex);
                label3.Text = "Status:";
                MessageBox.Show("Successfully unmounted");
            }
            else
            {

            }
            //MessageBox.Show(dataGridView1.CurrentCell.Value.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
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
                //MessageBox.Show(AutoUpdate);
                if (AutoUpdate.Contains("true"))
                {
                  //  MessageBox.Show("updatesection");
                    System.Net.NetworkInformation.Ping TestServer = new System.Net.NetworkInformation.Ping();
                    if (TestServer.Send("89.203.4.93", 300).Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                    //    MessageBox.Show("works");
                        System.Net.WebClient GetLatestInfo = new System.Net.WebClient();
                        GetLatestInfo.DownloadFile("ftp://89.203.4.93/downloads/isomount/latestversion.txt", "latestversion.txt");
                        CheckVersion = new StreamReader(Application.StartupPath + "\\" + "latestversion.txt");
                        String LatestFile = CheckVersion.ReadToEnd();
                        CheckVersion.Close();
                        output1 = LatestFile.Split(Splitter);
                        String LatestVersion = output1[0];
                        if (LatestVersion == CurrentVersion)
                        {

                        }
                        else
                        {
                            DialogResult UpdateResponse = MessageBox.Show("New version, " + output1[0] + ", from " + output1[1] + " found. Update?", "Update available", MessageBoxButtons.YesNoCancel);
                            if (UpdateResponse == DialogResult.Yes)
                            {
                                Form2 downloadform = new Form2();
                                downloadform.UpdateProgram();
                                downloadform.ShowDialog();

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("cannot contact update server");
                    }

                }
                else
                {
                    button3.Text = "Updates disabled";
                }

            
                }
            else
            {
                MessageBox.Show("Some files cannot be found. The program may run without them however cannot be updated");
                DialogResult UpdateResponse = MessageBox.Show("Would you like to download the newest version from online?", "Download latest version", MessageBoxButtons.YesNo);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 updateform = new Form2();
            updateform.ShowDialog();
        }
    }
}
