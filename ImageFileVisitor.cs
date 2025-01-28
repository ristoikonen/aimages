using aimages.Models;
using System.IO;

namespace aimages
{
    public class ImageFileVisitor
    {
        const string TXT_EXT= ".txt";
        private string PathToFiles { get; set; }
        private string PathToSubDirImageTextFiles { get; set; }

        public ImageFileVisitor(string pathToImageFiles, string pathToSubDirImageTextFiles)
        {
            PathToFiles= pathToImageFiles;
            PathToSubDirImageTextFiles = pathToSubDirImageTextFiles;
        }

        // Create AImage object by reading names, filepaths and descrption file (sits in a subdir)
        public List<AImage> ReadDirectory(string pathToFiles) 
        {
            List<AImage> list = new List<AImage>();
            string desc = "";

            foreach (var filename in Directory.GetFiles(pathToFiles))
            {
                string subdirForTexFiles = Directory.GetDirectories(pathToFiles, PathToSubDirImageTextFiles).FirstOrDefault<string>() ?? "";
                // any filetype, root
                //var filenames = Directory.GetFiles(pathToFiles, "*", SearchOption.AllDirectories).Select(x => Path.GetFileName(x)).ToArray();

                //foreach (var textfilename in filenames)
                //{
                // any filetype, subdir
                //var textfile = System.IO.Directory.GetFiles(subdirForTexFiles, textfilename + ".*", System.IO.SearchOption.AllDirectories).FirstOrDefault<string>() ?? "";

                var textfile = Path.Combine(subdirForTexFiles, Path.GetFileNameWithoutExtension(filename) + TXT_EXT );

                //var textfile = System.IO.Directory.GetFiles(subdirForTexFiles, textfilename + ".*", System.IO.SearchOption.AllDirectories).FirstOrDefault<string>() ?? "";

                if (File.Exists(textfile))
                    {
                        // Console.WriteLine("Reading:" + textfile);
                        desc = File.ReadAllText(textfile) ?? "";
                    }
                //}
                list.Add(new AImage(Path.GetFileName(filename) ?? "", filename, desc));
            }
            return list;

        }
    }
}
