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
    }
}
