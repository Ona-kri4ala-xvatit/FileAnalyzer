using FileAnalyzer.ViewModel.Base;
using System.IO;
using System;
using System.Collections.ObjectModel;

namespace FileAnalyzer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<string> Files { get; set; } = new ObservableCollection<string>();

        private ObservableCollection<string> selectedFile;
        public ObservableCollection<string> SelectedFile { get => selectedFile; set => base.PropertyChangeMethod(out selectedFile, value); }

        public MainViewModel()
        {
            SelectedFile = new ObservableCollection<string>();
            CheckDirectory();
        }

        public void CheckDirectory()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(directory);

            foreach (var item in files)
            {
                if (item.Contains(".json") || item.Contains(".txt"))
                {
                    Files.Add(Path.GetFileName(item));
                }
            }
        }
    }
}
