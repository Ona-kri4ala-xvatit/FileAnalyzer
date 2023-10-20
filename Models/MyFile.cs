namespace FileAnalyzer.Models
{
    public class MyFile
    {
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public MyFile() { }
        public MyFile(string? filePath, string? fileName)
        {
            FilePath = filePath;
            FileName = fileName;
        }
        public override string ToString()
        {
            return $"{FileName}";
        }

    }
}
