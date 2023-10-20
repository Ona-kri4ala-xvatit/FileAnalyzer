namespace FileAnalyzer.Models
{
    public class MyFileInfo
    {
        public int Sentences { get; set; }

        public MyFileInfo() { }

        public MyFileInfo(int sentences)
        {
            Sentences = sentences;
        }

        public override string ToString()
        {
            return $"Sentences: {Sentences}";
        }
    }
}
