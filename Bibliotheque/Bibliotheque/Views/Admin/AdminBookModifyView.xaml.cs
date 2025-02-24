﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bibliotheque.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour AdminBookModifyView.xaml
    /// </summary>
    public partial class AdminBookModifyView : ContentControl
    {
        public AdminBookModifyView()
        {
            InitializeComponent();
        }

        private void BookStateCopyQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new();
            fileDialog.Filter = "Image File (*.jpg)|*.jpg|(*.jpeg)|*.jpeg|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {

                FilePath.Text = fileDialog.FileName;
            }
        }
    }
}
