namespace LighthouseWeb.Models
{
    public class FileDetails
    {
        public string Directory { get; set; }

        public long Length { get; set; }

        public string Name { get; set; }

        public int LineCount { get; set; }

        public int WordCount { get; set; }

        public string FileContent { get; set; }
    }
}
