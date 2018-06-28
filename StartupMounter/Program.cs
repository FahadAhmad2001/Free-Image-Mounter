using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StartupMounter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Automated startup image mounter for free image mounter");
            StreamReader GetPathNames;
            string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            var directory = System.IO.Path.GetDirectoryName(path);
            GetPathNames = new StreamReader(directory + "\\" + "startuplist.txt");
            string contents = GetPathNames.ReadToEnd();
            String[] split1;
            Char splitter = ':';
            split1 = contents.Split(splitter);
            for(int count = 0; count < split1.Length + 1; count++)
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
                MountImageInfo.Arguments = "/c " + command2;
                MountImage.StartInfo = MountImageInfo;

            }
            Console.ReadLine();
        }
    }
}
