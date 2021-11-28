﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWorkMailClient.FolderItems
{
    /// <summary>
    /// Interaction logic for RenameFolderWindow.xaml
    /// </summary>
    public partial class RenameFolderWindow : Window
    {
        public string NewFolderName
        {
            get => tbNameFolder.Text;
        }

        public RenameFolderWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
