using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScheduledFileMover
{
    class Program
    {
        static void Main()
        {
            string sourceFolder = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Current Models\";
            string destinationFolder = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Previous Models\";
            string logPath = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Copy_Activity_Log.txt";

            StreamWriter stream = new StreamWriter(logPath, true);
            string logTimeStamp = "Scheduled task executed at: " + DateTime.Now.ToString("HH:mm:ss, MMMM dd, yyyy");
            stream.WriteLine("----------------------------------------------------------------------------------");
            stream.Write(logTimeStamp);
            stream.WriteLine();
            try
            {

                if (System.IO.Directory.Exists(sourceFolder))
                {
                    string[] files = System.IO.Directory.GetFiles(sourceFolder);
                    
                    if (files.Length == 0)
                    {
                        stream.WriteLine("No files present in folder at time of scheduled task. Nothing copied.");
                        stream.WriteLine();
                    }
                    else
                    {
                        stream.WriteLine("List of file(s) successfully moved:");
                    }

                    int i = 1;
                    foreach (string file in files)
                    {
                        // This does not recursively copy subfolders and their respective files
                        stream.WriteLine("\t"+i+". "+file);
                        string fileName = System.IO.Path.GetFileName(file);
                        string destFile = System.IO.Path.Combine(destinationFolder, fileName);
                        System.IO.File.Copy(file, destFile, true);
                        System.IO.File.Delete(file);
                        i++;
                    }
                }
                else
                {
                    stream.WriteLine("Source folder or files do not exist. Nothing moved.");
                }
            }
            catch (Exception e)
            {
                stream.WriteLine("An error occured:");
                stream.WriteLine(e.ToString());
            }
            stream.Close();
        }
    }
}
