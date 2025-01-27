namespace aimages.Models
{
    public class AImage
    {
        private const string DESC_DIR = "Descriptions";

        public string Name { get; set; }
        public string FileName { get; set; }

        public string Desc { get; set; }

        public AImage()
        {

        }

        public AImage(string name, string fileName)
        {
            this.Name = name;
            this.FileName = fileName;
            this.Desc = GetDesc(fileName);
        }

        public AImage(string name, string fileName, string desc)
        {
            this.Name = name;
            this.FileName = fileName;
            this.Desc = GetDesc(fileName);
        }

        private string GetDesc(string? fileName)
        {
            var directoryName = Path.GetDirectoryName(fileName ?? @"c:\tmp\images");
            if (fileName == null || directoryName == null || !Path.Exists(directoryName))
            {
                return "";
            }

            var descdir = directoryName + @"\" + DESC_DIR;
            var descfile = Path.Combine(descdir, Path.GetFileNameWithoutExtension(fileName)) + @".txt";

            var descpath = Path.Combine(descdir, descfile);

            if (File.Exists(descpath))
                return File.ReadAllText(descpath);
            else
                return "";
        }
    }
}
