using FileAnalyzer.Base;
using FileAnalyzer.Commands;
using FileAnalyzer.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private CancellationTokenSource? tokenSource;
        #region Properties   
        public ObservableCollection<MyFile> Files { get; set; }

        public ObservableCollection<MyFileInfo>? infos;
        public ObservableCollection<MyFileInfo>? Infos { get => infos; set => base.PropertyChangeMethod(out infos, value); }

        private MyFile? selectedFile;
        public MyFile? SelectedFile { get => selectedFile; set => base.PropertyChangeMethod(out selectedFile, value); }

        private int progressBarValue;
        public int ProgressBarValue { get => progressBarValue; set => PropertyChangeMethod(out progressBarValue, value); }
        #endregion

        #region Commands
        private CommandBase? analyzeCommand;
        public CommandBase AnalyzeCommand => analyzeCommand ??= new CommandBase(
            execute: async () =>
            {
                tokenSource = new CancellationTokenSource();
                await CheckerState(tokenSource.Token);
            },
            canExecute: () => true);

        private CommandBase? cancelCommand;
        public CommandBase CancelCommand => cancelCommand ??= new CommandBase(
            execute: () =>
            {
                tokenSource?.Cancel();
                if(cancelCommand is not null)
                    tokenSource = null;
            },
            canExecute: () => true);
        #endregion

        public MainViewModel()
        {
            Files = new ObservableCollection<MyFile>();
            SelectedFile = new MyFile();
            Infos = new ObservableCollection<MyFileInfo>();
            CheckDirectory();
        }

        #region Methods
        private async Task CheckerState(object state)
        {
            var token = (CancellationToken)state;

            while (!token.IsCancellationRequested)
            {
                if (ProgressBarValue != 100)
                {
                    ProgressBarValue++;
                    await Task.Delay(20);
                }
                else
                {
                    tokenSource.Cancel();
                    AnalyzeText();
                }

                if (token.IsCancellationRequested == true)
                {
                    await Task.Delay(100);
                    ProgressBarValue = 0;
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

            //Infos?.Clear();
            Infos?.Add(new MyFileInfo(wordsCount, symbolsCount, sentences));
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
        #endregion
    }
}
