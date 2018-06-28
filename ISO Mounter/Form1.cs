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
using Microsoft.Win32;
//using System.Runtime.

namespace ISO_Mounter
{
    public partial class Form1 : Form
    {
        public string FileLocation;
        public string command1;
        public string command2;
        public StreamWriter editBAT;
        public StreamWriter log;
        public String LogStatus;
        public string output1;
        public string error1;
        public string RunOnStartup;
        public String AutoUpdate;
        public String CurrentVersion;
        public String CurrentVDate;
        public static StringBuilder Logs;
        public String[] split2;
        public String[] split1;
        //public int unmountresponse;
        //public Process MountImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("fool");
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

        private void button2_Click1(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            //MessageBox.Show(dataGridView1.CurrentCell.Value.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(AppClosing);
            if (File.Exists(Application.StartupPath + "\\" + "log.txt"))
            {
                File.Delete(Application.StartupPath + "\\" + "log.txt");
            }
            if (File.Exists(Application.StartupPath +"\\" + "exitlog.txt"))
            {
                File.Delete(Application.StartupPath + "\\" + "exitlog.txt");
            }
            richTextBox1.AppendText("------------Free Image Mounter------------");
            Logs = new StringBuilder();
            Logs.Append("------------Free Image Mounter------------");
            if (File.Exists(Application.StartupPath + "\\" + "version.txt"))
            {
                if(File.Exists(Application.StartupPath + "\\" + "config.ini"))
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       config.ini is present");
                    Logs.Append(Environment.NewLine + DateTime.Now + "       config.ini is present");
                    StreamReader ReadConfig;
                    ReadConfig = new StreamReader(Application.StartupPath + "\\" + "config.ini");
                    String FullConfig;
                    FullConfig = ReadConfig.ReadToEnd();
                   
                    Char Seperation = '&';
                    split1 = FullConfig.Split(Seperation);
                    Char Seperation2 = ':';
                    //String[] split2;
                    split2 = split1[0].Split(Seperation2);
                    if (split2[0].Contains("True"))
                    {
                        RunOnStartup = "TRUE";
                        button4.Text = "Dont run at startup";
                    }
                    if (split2[0].Contains("False"))
                    {
                        RunOnStartup = "FALSE";
                        button4.Text = "Run at startup";
                    }
                }
                else
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Error: cannot find config.ini");
                    Logs.Append(Environment.NewLine + DateTime.Now + "       Error: cannot find config.ini");
                    
                }
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       version.txt is present");
                Logs.Append(Environment.NewLine + DateTime.Now + "       version.txt is present");
                StreamReader CheckVersion;
                CheckVersion = new StreamReader(Application.StartupPath + "\\" + "version.txt");
                String FullFile = CheckVersion.ReadToEnd();
                CheckVersion.Close();
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Successfully read version.txt");
                Logs.Append(Environment.NewLine + DateTime.Now + "       Successfully read version.txt");
                String[] output1;
                Char Splitter = ':';
                output1 = FullFile.Split(Splitter);
                CurrentVersion = output1[0];
                CurrentVDate = output1[1];
                AutoUpdate = output1[2];
                LogStatus = output1[3];
                if (LogStatus.Contains("true"))
                {
                    label5.Text = "Logging is currently enabled. The tickbox is to change logging status, regardless of whether it is ticked or not";
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Starting to write to log file");
                    Logs.Append(Environment.NewLine + DateTime.Now + "       Starting to write to log file");
                    timer1.Start();
                }
                else
                {
                    label5.Text = "Logging is currently disabled. The tickbox is to change logging status, regardless of whether it is ticked or not";
                }
                AutoUpdate.TrimEnd('\r', '\n');
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Free Image Mounter v" + CurrentVersion + ", from " + CurrentVDate);
                Logs.Append(Environment.NewLine + DateTime.Now + "       Free Image Mounter v" + CurrentVersion + ", from " + CurrentVDate);
                //MessageBox.Show(AutoUpdate);
                if (AutoUpdate.Contains("true"))
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Autoupdates enabled");
                    Logs.Append(Environment.NewLine + DateTime.Now + "       Autoupdates enabled");
                    if (File.Exists(Application.StartupPath + "\\" + "latestversion.txt"))
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Found obsolete latestversion.txt");
                        Logs.Append(Environment.NewLine + DateTime.Now + "       Found obsolete latestversion.txt");
                        File.Delete(Application.StartupPath + "\\" + "latestversion.txt");
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Deleted old latestversion.txt");
                        Logs.Append(Environment.NewLine + DateTime.Now + "       Deleted old latestversion.txt");
                    }
                  //  MessageBox.Show("updatesection");
                    System.Net.NetworkInformation.Ping TestServer = new System.Net.NetworkInformation.Ping();
                    if (TestServer.Send("89.203.4.93", 300).Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Can successfully ping update server");
                        Logs.Append(Environment.NewLine + DateTime.Now + "       Can successfully ping update server");
                        //    MessageBox.Show("works");
                        System.Net.WebClient GetLatestInfo = new System.Net.WebClient();
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Trying to download update config file");
                        Logs.Append(Environment.NewLine + DateTime.Now + "       Trying to download update config file");
                        GetLatestInfo.DownloadFile("ftp://89.203.4.93:2048/downloads/isomount/latestversion.txt", "latestversion.txt");
                        if (File.Exists(Application.StartupPath + "\\" + "latestversion.txt"))
                        {
                            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Successfully downloaded update config files");
                            Logs.Append(Environment.NewLine + DateTime.Now + "       Successfully downloaded update config files");
                        }
                        else
                        {
                            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Error in downloading update files, skipping autoupdate");
                            Logs.Append(Environment.NewLine + DateTime.Now + "       Error in downloading update files, skipping autoupdate");
                            goto EndCheckUpdate;
                        }
                        CheckVersion = new StreamReader(Application.StartupPath + "\\" + "latestversion.txt");
                        String LatestFile = CheckVersion.ReadToEnd();
                        CheckVersion.Close();
                        output1 = LatestFile.Split(Splitter);
                        String LatestVersion = output1[0];
                        if (LatestVersion == CurrentVersion)
                        {
                            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Current version is already up to date");
                            Logs.Append(Environment.NewLine + DateTime.Now + "       Current version is already up to date");
                        }
                        else
                        {
                            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       New version found");
                            Logs.Append(Environment.NewLine + DateTime.Now + "       New version found");
                            DialogResult UpdateResponse = MessageBox.Show("New version, " + output1[0] + ", from " + output1[1] + " found. Update?", "Update available", MessageBoxButtons.YesNoCancel);
                            if (UpdateResponse == DialogResult.Yes)
                            {
                                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Starting download app");
                                Logs.Append(Environment.NewLine + DateTime.Now + "       Starting download app");
                                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Update logs are found in update.txt, if enabled");
                                Logs.Append(Environment.NewLine + DateTime.Now + "       Update logs are found in update.txt, if enabled");
                                Form2 downloadform = new Form2();
                                downloadform.UpdateProgram();
                                downloadform.ShowDialog();

                            }
                        }

                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Cannot ping update server, skipping autoupdate");
                        Logs.Append(Environment.NewLine + DateTime.Now + "       Cannot ping update server, skipping autoupdate");
                    }

                }
                else
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Autoupdates disabled");
                    Logs.Append(Environment.NewLine + DateTime.Now + "       Autoupdates disabled");
                    button3.Text = "Updates disabled";
                }

                EndCheckUpdate:;
                }
            else
            {
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Error: cannot find version.txt");
                Logs.Append(Environment.NewLine + DateTime.Now + "       Error: cannot find version.txt");
                EndStartup:;
                MessageBox.Show("Some files cannot be found. The program may run without them however cannot be updated and logs might not work");
                DialogResult UpdateResponse = MessageBox.Show("Would you like to download the newest version from online?", "Download latest version", MessageBoxButtons.YesNo);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 updateform = new Form2();
            updateform.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                DialogResult ChangeLogging = MessageBox.Show("This will be applicable from the next time the application restarts. Continue?", "Enable logging?", MessageBoxButtons.YesNoCancel);
                if (ChangeLogging == DialogResult.Yes)
                {
                    String command1 = CurrentVersion + ":" + CurrentVDate + ":" + AutoUpdate + ":true";
                    StreamWriter ChangeLogStatus;
                    ChangeLogStatus = new StreamWriter(Application.StartupPath + "\\" + "version.txt");
                    ChangeLogStatus.Write(command1);
                    ChangeLogStatus.Close();
                }
            }
            if (!checkBox1.Checked)
            {
                DialogResult ChangeLogging = MessageBox.Show("This will be applicable from the next time the application restarts. Continue?", "Disable logging?", MessageBoxButtons.YesNoCancel);
                if (ChangeLogging == DialogResult.Yes)
                {
                    String command1 = CurrentVersion + ":" + CurrentVDate + ":" + AutoUpdate + ":false";
                    StreamWriter ChangeLogStatus;
                    ChangeLogStatus = new StreamWriter(Application.StartupPath + "\\" + "version.txt");
                    ChangeLogStatus.Write(command1);
                    ChangeLogStatus.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (LogStatus.Contains("true"))
            {
                File.AppendAllText(Application.StartupPath + "\\" + "log.txt", Logs.ToString());
            }
            
            Logs.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            label3.Text = "Status: Selecting file";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileLocation = openFileDialog1.FileName;
                label2.Text = FileLocation;
                label3.Text = "Status: ";
            }
            else
            {
                label3.Text = "Status: ";
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            command1 = "Mount-DiskImage -ImagePath " + "'" + FileLocation + "'";
            //using (PowerShell PowerShellInstance = PowerShell.Create())
            //{
            //  PowerShellInstance.AddCommand("Mount-DiskImage");
            //PowerShellInstance.AddParameter();
            //PowerShellInstance.AddArgument(" -ImagePath " + "'" + FileLocation + "'");
            // PowerShellInstance.Invoke();

            //}
            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Writing commands to PowerShell script");
            Logs.Append(Environment.NewLine + DateTime.Now + "       Writing commands to PowerShell script");
            editBAT = new StreamWriter(Application.StartupPath + "\\" + "MountISO.ps1");
            editBAT.WriteLine(command1);
            editBAT.Close();
            command2 = "powershell.exe -command " + command1;
            //MessageBox.Show(command2);
            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Generating mount process");
            Logs.Append(Environment.NewLine + DateTime.Now + "       Generating mount process");
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
            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Starting mount process");
            Logs.Append(Environment.NewLine + DateTime.Now + "       Starting mount process");
            MountImage.Start();
            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Mount process successfully started with ID " + MountImage.Id.ToString());
            Logs.Append(Environment.NewLine + DateTime.Now + "       Mount process successfully");
            //output1 = MountImage.StandardOutput.ReadToEnd();
            //error1 = MountImage.StandardError.ReadToEnd();
            //MessageBox.Show(output1);
            //MessageBox.Show(error1);
            MountImage.WaitForExit();
            MountImage.Close();
            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Mount process completed");
            Logs.Append(Environment.NewLine + DateTime.Now + "       Mount process completed");
            dataGridView1.Rows.Add(FileLocation);
            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Image path added to table");
            Logs.Append(Environment.NewLine + DateTime.Now + "       Image path added to table");
            label3.Text = "Status:";
            MessageBox.Show("Successfully mounted");
            richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Image mounted");
            Logs.Append(Environment.NewLine + DateTime.Now + "       Image mounted");
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult unmountresponse = MessageBox.Show("Do you really want to unmount this image", "Confirm dismount", MessageBoxButtons.YesNo);
            if (unmountresponse == DialogResult.Yes)
            {
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Generating commands");
                Logs.Append(Environment.NewLine + DateTime.Now + "       Generating commands");
                int CellIndex = dataGridView1.CurrentCell.RowIndex;
                String removeFile = dataGridView1.CurrentCell.Value.ToString();
                String command3 = "Dismount-DiskImage -ImagePath " + "'" + removeFile + "'";
                String command4 = "/c powershell.exe -command " + command3;
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Generating unmount process");
                Logs.Append(Environment.NewLine + DateTime.Now + "       Generating unmount process");
                System.Diagnostics.Process UnMountImage = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo UnMountImageInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                UnMountImageInfo.CreateNoWindow = true;
                UnMountImageInfo.UseShellExecute = false;
                UnMountImageInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                UnMountImageInfo.Arguments = command4;
                UnMountImage.StartInfo = UnMountImageInfo;
                label3.Text = "Status: Unmounting";
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Starting unmount process");
                Logs.Append(Environment.NewLine + DateTime.Now + "       Starting unmount process");
                UnMountImage.Start();
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Unmount process started successfully with ID " + UnMountImage.Id.ToString());
                Logs.Append(Environment.NewLine + DateTime.Now + "       Unmount process started successfully");
                UnMountImage.WaitForExit();
                UnMountImage.Close();
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Unmount process completed");
                Logs.Append(Environment.NewLine + DateTime.Now + "       Unmount process completed");
                dataGridView1.Rows.RemoveAt(CellIndex);
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now + "       Image path removed from table");
                Logs.Append(Environment.NewLine + DateTime.Now + "       Image path removed from table");
                label3.Text = "Status:";
                MessageBox.Show("Successfully unmounted");
            }
            else
            {

            }
        }
        private void Form1_Closing(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           // if (LogStatus.Contains("true"))
           // {
            //    File.AppendAllText(Application.StartupPath + "\\" + "log.txt", Logs.ToString());
            //}
            
        }
        static void AppClosing(object Sender, EventArgs e)
        {
            StreamWriter logwriter;
            logwriter = new StreamWriter(Application.StartupPath + "\\" + "exitlog.txt");
            logwriter.Write(Logs);
            logwriter.Close();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (RunOnStartup.Contains("FALSE"))
            {
                String Firsttext;
                Firsttext = "StartupEnabled=True:";
                for (int count = 0; count < split2.Length +1; count++)
                {
                    if (count > 0)
                    {
                        Firsttext = Firsttext + split2[count] + ":";
                    }
                }
                Firsttext = Firsttext + "&" + split1[1];
                StreamWriter ChangeConfig;
                ChangeConfig = new StreamWriter(Application.StartupPath + "\\" + "config.ini");
                ChangeConfig.Write(Firsttext);
                ChangeConfig.Close();
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.SetValue("Free Image Mounter", Application.StartupPath + "\\" + "StartupMounter.exe");
                RunOnStartup = "TRUE";
                goto EndChangeConfig;      
            }
            if (RunOnStartup.Contains("TRUE"))
            {
                String Firsttext;
                Firsttext = "StartupEnabled=False:";
                for (int count = 0; count < split2.Length + 1; count++)
                {
                    if (count > 0)
                    {
                        Firsttext = Firsttext + split2[count] + ":";
                    }
                }
                Firsttext = Firsttext + "&" + split1[1];
                StreamWriter ChangeConfig;
                ChangeConfig = new StreamWriter(Application.StartupPath + "\\" + "config.ini");
                ChangeConfig.Write(Firsttext);
                ChangeConfig.Close();
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.DeleteValue("Free Image Mounter", false);
                RunOnStartup = "FALSE";
                goto EndChangeConfig;
            }
            EndChangeConfig:;
        }
    }
}
