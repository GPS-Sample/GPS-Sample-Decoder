/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using GPSSampleDecoder.ViewModels;

namespace GPSSampleDecoder
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
      }
      private void PickFile_OnClick(object sender, EventArgs e)
      {
         var dialog = new OpenFileDialog();
         dialog.Multiselect = true;

         if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            ((MainWindowViewModel)(this.DataContext)).PathsToConfigurations.Clear();

            foreach (string fileName in dialog.FileNames)
            {
                ((MainWindowViewModel)(this.DataContext)).PathsToConfigurations.Add( fileName );
            }

            ((MainWindowViewModel)(this.DataContext)).setPathVars();

            this.FilePathTextBox.Text = dialog.FileName;
                
            // Since setting the property explicitly bypasses the data binding, 
            // we must explicitly update it by calling BindingExpression.UpdateSource()
            this.FilePathTextBox
              .GetBindingExpression(TextBlock.TextProperty)?
              .UpdateSource();
         }
      }

      private void PickDirectory_OnClick(object sender, EventArgs e)
      {
         var dialog = new FolderBrowserDialog(); //new OpenFileDialog(;
         if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            this.DecodePathTextBox.Text = dialog.SelectedPath;

            // Since setting the property explicitly bypasses the data binding, 
            // we must explicitly update it by calling BindingExpression.UpdateSource()
            this.DecodePathTextBox
              .GetBindingExpression(TextBlock.TextProperty)?
              .UpdateSource();
         }
      }

      private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         // We cancel the event and let the dialog handle it.
         e.Cancel = true;
         ((MainWindowViewModel)(this.DataContext)).Close = true;

      }

   }
}
