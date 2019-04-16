using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace ScheduledFileMover
{
    class Program
    {
        static void Main()
        {

            string configPath = @"C:\Users\benst\Desktop\ScheduledFileMoverPreferences.xml";
            PathData pd = new PathData();
            if (!File.Exists(configPath))
            {
                pd.SourceFolderPath = @"Enter a source folder path here\";
                pd.TargetFolderPath = @"Enter a destination folder path here\";
                pd.SavePathData(configPath);
                
            }
            else
            {
                Console.WriteLine("Logic B");
                Console.ReadLine();
            }


            // string sourceFolder = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Current Models\";
            // string destinationFolder = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Previous Models\";
            // string logPath = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Copy_Activity_Log.txt";

            
            // pd.SourceFolderPath = sourceFolder;
            // pd.TargetFolderPath = destinationFolder;
            // pd.SavePathData(@"D:\PathDataLog.txt");
            pd.ReloadPathData(@"C:\Users\benst\Desktop\XML_Preferences");

            string sourceFolder = pd.SourceFolderPath;
            string destinationFolder = pd.TargetFolderPath;
            string logPath = @"C:\Users\benst\Desktop\Log.txt";

            Console.WriteLine(sourceFolder.ToString());
            Console.WriteLine(destinationFolder.ToString());

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

    [Serializable]
    public class PathData
    {
        public string SourceFolderPath { get; set; }
        public string TargetFolderPath { get; set; }

        public void SavePathData(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                XmlSerializer xml = new XmlSerializer(typeof(PathData));
                TextWriter writer = new StreamWriter(filePath);
                xml.Serialize(stream, this);
            }
        }


        public void ReloadPathData(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                PathData APathData = new PathData();
                XmlSerializer xml = new XmlSerializer(typeof(PathData));
                APathData = (PathData)xml.Deserialize(stream);
                this.SourceFolderPath = APathData.SourceFolderPath;
                this.TargetFolderPath = APathData.TargetFolderPath;
            }
        }

    }
}
