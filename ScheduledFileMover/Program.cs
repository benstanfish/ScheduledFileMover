using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledFileMover
{
    class Program
    {
        static void Main()
        {
            string sourceFolder = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Current Models\";
            string destinationFolder = @"\\abam.com\Projects\FederalWay\2018\A18.0203\02\BIM\Collaboration\Previous Models\";

            if (System.IO.Directory.Exists(sourceFolder))
            {
                string[] files = System.IO.Directory.GetFiles(sourceFolder);

                foreach (string file in files)
                {
                    // This does not recursively copy subfolders and their respective files
                    string fileName = System.IO.Path.GetFileName(file);
                    string destFile = System.IO.Path.Combine(destinationFolder, fileName);
                    System.IO.File.Copy(file, destFile, true);
                    System.IO.File.Delete(file);
                }
            }
            else
            {
            }

        }
    }
}
