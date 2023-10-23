using FileAnalyzer.Base;
using FileAnalyzer.Commands;
using FileAnalyzer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace FileAnalyzer.ViewModels
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Properties   
        public ObservableCollection<MyFile> Files { get; set; } = new ObservableCollection<MyFile>();

        public ObservableCollection<MyFileInfo>? infos;
        public ObservableCollection<MyFileInfo>? Infos { get => infos; set => base.PropertyChangeMethod(out infos, value); }

        private MyFile? selectedFile;
        public MyFile? SelectedFile { get => selectedFile; set => base.PropertyChangeMethod(out selectedFile, value); }
        
        private double progressBarValue;
        public double ProgressBarValue { get => progressBarValue; set => base.PropertyChangeMethod(out progressBarValue, value); }
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

        public ICommand StartCommand { get; }


        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnPropertyChanged();
            }
        }

        private Thread _thread;
        private CancellationTokenSource _tokenSource;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            SelectedFile = new MyFile();
            Infos = new ObservableCollection<MyFileInfo>();
            CheckDirectory();

            StartCommand = new DelegateCommand((p) =>
            {
                _tokenSource = new CancellationTokenSource();
                _thread = new Thread(Worker) { IsBackground = true };
                _thread.Start(_tokenSource.Token);
            },
           p => _thread == null);

            //_tokenSource.Cancel();
            _tokenSource = null;
            _thread = null;

        }

        private void Worker(object state)
        {
            var token = (CancellationToken)state;
            while (!token.IsCancellationRequested)
            {
                Value++;
                Thread.Sleep(100);
            }
            //AnalyzeText();
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
            //new Thread(() =>
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        Application.Current.Dispatcher.Invoke(() =>
            //        {
            //            ProgressBarValue = i;
            //        });
            //        Thread.Sleep(100);
            //    } 
            //}).Start();

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
