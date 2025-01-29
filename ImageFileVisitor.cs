using aimages.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Extensions;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

            foreach (var fileNameAndPath in Directory.GetFiles(pathToFiles))
            {
                if (File.Exists(fileNameAndPath))
                {
                    string subdirForTexFiles = Directory.GetDirectories(pathToFiles, PathToSubDirImageTextFiles).FirstOrDefault<string>() ?? "";
                    if (Path.Exists(subdirForTexFiles))
                    { 
                        var textfile = Path.Combine(subdirForTexFiles, Path.GetFileNameWithoutExtension(fileNameAndPath) + TXT_EXT);
                        if (File.Exists(textfile))
                        {
                            desc = File.ReadAllText(textfile) ?? "";
                        }
                    }

                    AImage aimg = new AImage(Path.GetFileName(fileNameAndPath) ?? "", fileNameAndPath, desc, System.IO.File.ReadAllBytes(fileNameAndPath));
                    list.Add(aimg);
                }
            }
            return list;

        }

        public string GetImageAsBase64(string pathToImage)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(pathToImage);
            return Convert.ToBase64String(imageArray);
            //var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64String)));
            //Encoding.ASCII.GetBytes(Convert.ToBase64String(File.ReadAllBytes(pathToImage)));
        }


        public static List<AImage> GetImages(string pathToImages)
        {
            List<string> images = new List<string>();
            List<AImage> aimages = new List<AImage>();

            foreach (string file in Directory.GetFiles(pathToImages))
            {
                // Path.GetFileName(
                //if (file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))
                var filenamewithpath = Path.GetFileName(pathToImages) + @"\" + file;

                AImage aimage = new AImage(file, filenamewithpath , "", System.IO.File.ReadAllBytes(filenamewithpath));
                aimages.Add(aimage);
            }
            return aimages;
        }
        

        public static List<AImage> GetImagesOld(string path)
        {
            List<string> images = new List<string>();
            List<AImage> aimages = new List<AImage>();

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                // Path.GetFileName(
                //if (file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))

                AImage aimage = new AImage(Path.GetFileName(file), file);
                aimages.Add(aimage);
            }
            return aimages;
        }


    }
}
