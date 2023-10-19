using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace FileAnalyzer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            CheckDirectory();
        }


        private void CheckDirectory()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            List<string> files = new List<string>(Directory.GetFiles(directory));

            foreach (var item in files)
            {
                if (item.Contains(".json") || item.Contains(".txt"))
                {

                    FilesListBox.Items.Add(Path.GetFileName(item));

                }
            }

        }
    }
}
