using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace ScheduledFileMover
{
    class Program
    {
        static void Main()
        {

            string aPath = Path.GetDirectoryName(Application.ExecutablePath);
            
            string configPath = aPath + @"\Preferences.xml";
            string logPath = aPath + @"\Log.txt";
            PathData pd = new PathData();
            
            if (!File.Exists(configPath))
            {
                CreateNewPreferencesFile(configPath);
            }
            else
            {
                StreamWriter stream = new StreamWriter(logPath, true);
                try
                {
                    pd.ReloadPathData(configPath);
                    string sourceFolder = pd.SourceFolderPath;
                    string destinationFolder = pd.TargetFolderPath;
                    
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
                                stream.Write("  " + i + ". " + file);
                                string fileName = System.IO.Path.GetFileName(file);
                                string destFile = System.IO.Path.Combine(destinationFolder, fileName);
                                stream.WriteLine(" was moved to " + destFile);
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
                    
                }
                catch (Exception e)
                {
                    stream.WriteLine("An error occured:");
                    stream.WriteLine(e.ToString());
                }
                stream.Close();
            }

            
        }


        static void CreateNewPreferencesFile(string configPath)
        {
            PathData newConfig = new PathData();
            newConfig.SourceFolderPath = @"Insert full source folder path here";
            newConfig.TargetFolderPath = @"Insert full destination folder path here";

            using (FileStream stream = new FileStream(configPath, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(PathData));
                xml.Serialize(stream, newConfig);
            }
        }
    }

    [Serializable]
    public class PathData
    {
        public string SourceFolderPath { get; set; }
        public string TargetFolderPath { get; set; }

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
