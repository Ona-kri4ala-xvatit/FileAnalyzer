namespace FileAnalyzer.Models
{
    public class MyFileInfo
    {
        public int WordsCount { get; set; }
        public int SymbolsCount { get; set; }
        public int Sentences { get; set; }

        public MyFileInfo() { }

        public MyFileInfo(int wordsCount, int symbolsCount, int sentences)
        {
            WordsCount = wordsCount;
            SymbolsCount = symbolsCount;
            Sentences = sentences;
        }

        public override string ToString()
        {
            return $"Words count: {WordsCount}\nSymbols count: {SymbolsCount}\nSentences: {Sentences}";
        }
    }
}
