using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace FileAnalyzer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(dir);

            if(files.Length > 0)
            {
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
            else
            {
                Console.WriteLine("No files");
            }
        }
    }
}
