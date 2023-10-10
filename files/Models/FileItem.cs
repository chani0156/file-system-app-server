namespace files.Models
{
    public class FileItem
    {
        public string Name { get; set; }
        public List<string> Files { get; set; }
        public List<List<FileItem>> Directories { get; set; }
    }
}
