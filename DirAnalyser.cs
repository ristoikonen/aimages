using aimages.Models;

namespace aimages
{
    public static class DirAnalyser
    {
        public static List<AImage> GetImages(string path)
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
