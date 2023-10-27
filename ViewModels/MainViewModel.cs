using FileAnalyzer.Base;
using FileAnalyzer.Commands;
using FileAnalyzer.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows.Input;

namespace FileAnalyzer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Thread thread;
        private CancellationTokenSource tokenSource;

        #region Properties   
        public ObservableCollection<MyFile> Files { get; set; } = new ObservableCollection<MyFile>();

        public ObservableCollection<MyFileInfo>? infos;
        public ObservableCollection<MyFileInfo>? Infos { get => infos; set => base.PropertyChangeMethod(out infos, value); }

        private MyFile? selectedFile;
        public MyFile? SelectedFile { get => selectedFile; set => base.PropertyChangeMethod(out selectedFile, value); }

        private int progressBarValue;
        public int ProgressBarValue
        {
            get => progressBarValue;
            set => PropertyChangeMethod(out progressBarValue, value);
            //if (value == progressBarValue) return;
            //progressBarValue = value;
        }
        #endregion

        #region Commands
        private CommandBase? analyzeCommand;
        public CommandBase AnalyzeCommand => analyzeCommand ??= new CommandBase(
            execute: () =>
            {
                tokenSource = new CancellationTokenSource();
                thread = new Thread(Worker) 
                { 
                    IsBackground = true 
                };
                thread.Start(tokenSource.Token);

                //tokenSource.Cancel();
                tokenSource = null;
                thread = null;
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
        private void Worker(object state)
        {
            var token = (CancellationToken)state;
            while (!token.IsCancellationRequested)
            {
                ProgressBarValue++;
                Thread.Sleep(20);
            }
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

            //this.ProgressBarValue = 0;
        }
        #endregion
    }
}
