using System.IO;
using System;
using System.Collections.ObjectModel;
using FileAnalyzer.Models;
using FileAnalyzer.ViewModels.Base;
using System.Windows;
using System.Linq;
using System.Text.Json;
using FileAnalyzer.Commands;

namespace FileAnalyzer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties   
        public ObservableCollection<MyFile> Files { get; set; } = new ObservableCollection<MyFile>();

        public ObservableCollection<MyFileInfo>? infos;
        public ObservableCollection<MyFileInfo>? Infos { get => infos; set => base.PropertyChangeMethod(out infos, value); }

        private MyFile? selectedFile;
        public MyFile? SelectedFile { get => selectedFile; set => base.PropertyChangeMethod(out selectedFile, value); }
        #endregion

        #region Commands
        private CommandBase? analyzeCommand;
        public CommandBase AnalyzeCommand => analyzeCommand ??= new CommandBase(
            execute: () =>
            {
                AnalyzeText();
            },
            canExecute: () => true);
        #endregion

        public MainViewModel()
        {
            SelectedFile = new MyFile();
            Infos = new ObservableCollection<MyFileInfo>();
            CheckDirectory();
        }

        #region Methods
        private void CheckDirectory()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(directory);

            foreach (var item in files)
            {
                if (item.Contains(".json") || item.Contains(".txt"))
                {
                    Files.Add(new MyFile(Path.GetFullPath(item), Path.GetFileName(item)));
                }
            }
        }

        private void AnalyzeText()
        {
            if (SelectedFile?.FilePath is null)
                return;

            string text = File.ReadAllText(SelectedFile.FilePath);

            int wordsCount = text.Split(new char[] { ' ', '\t', '\n', '\r', '\u0022', ':', ',', '=', '.', '{', '}', '[', ']' },
                StringSplitOptions.RemoveEmptyEntries).Length;

            int symbolsCount = text.Replace(" ", string.Empty).Length;

            int sentences = text.Split('.', '!', '?', (char)StringSplitOptions.RemoveEmptyEntries).Length;

            Infos?.Clear();
            Infos?.Add(new MyFileInfo(wordsCount, symbolsCount, sentences));
        }
        #endregion
    }
}
