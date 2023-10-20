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
                AnalyzeSentences();
            },
            canExecute: () => true);
        #endregion


        public MainViewModel()
        {
            SelectedFile = new MyFile();
            Infos = new ObservableCollection<MyFileInfo>();
            CheckDirectory();
        }
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

        private void AnalyzeSentences()
        {
            if (SelectedFile?.FilePath is null)
                return;

            string text = File.ReadAllText(SelectedFile.FilePath);

            int sentences = text.Split(new char[] { ' ', '\t', '\n', '\r', '\u0022', ':', ',', '=', '.', '{', '}', '[', ']' },
                StringSplitOptions.RemoveEmptyEntries).Length;

            Infos?.Clear();
            Infos?.Add(new MyFileInfo(sentences));
        }

    }
}
