using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace StartupMounter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Automated startup image mounter for free image mounter");
            StreamReader GetPathNames;
            string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string location = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string directory = Path.GetDirectoryName(location);
            //String[] split2;
            //split2 = Regex.Split(directory, "file:");
            //Char removed = '\\';
            //split2[1].TrimStart(removed);
            GetPathNames = new StreamReader(directory + "\\" + "startuplist.txt");
            string contents = GetPathNames.ReadToEnd();
            GetPathNames.Close();
            String[] split1;
            Char splitter = '*';
            split1 = contents.Split(splitter);
            for(int count = 0; count < split1.Length; count++)
            {
                Console.WriteLine("Image path is " + split1[count]);
                String command1 = "Mount-DiskImage -ImagePath " + "'" + split1[count] + "'";
                String command2= "powershell.exe -command " + command1;
                Console.WriteLine(command2);
                System.Diagnostics.Process MountImage = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo MountImageInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                MountImageInfo.CreateNoWindow = true;
                MountImageInfo.UseShellExecute = false;
                MountImageInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                MountImageInfo.RedirectStandardError = true;
                MountImageInfo.RedirectStandardOutput = true;
                MountImageInfo.Arguments = "/c " + command2;
                MountImage.StartInfo = MountImageInfo;
                Console.WriteLine("Starting process");
                MountImage.Start();
                String output1 = MountImage.StandardOutput.ReadToEnd();
                String error1 = MountImage.StandardError.ReadToEnd();
                MountImage.WaitForExit();
                MountImage.Close();
                Console.WriteLine("Output:" + output1);
                Console.WriteLine("Errors, if any:" + error1);
            }
            Console.WriteLine("no more images to mount, exiting");
            
        }
    }
}
