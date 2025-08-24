/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.Static;
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
using System.Windows.Shapes;

namespace GPSSampleDecoder.Views
{
   /// <summary>
   /// Interaction logic for Window1.xaml
   /// </summary>
   public partial class PasswordDialog : Window
   {
      private string password;
      private PasscodeReturn passcodeReturn;
      public PasswordDialog()
      {
         passcodeReturn = new PasscodeReturn();
         InitializeComponent();
      }
      public PasscodeReturn ShowPasswordDialog()
      {
         this.ShowDialog();
         return passcodeReturn;
      }

      private void okButton_Click(object sender, RoutedEventArgs e)
      {
         passcodeReturn.passcode = PasswordTextBox.Password;
         passcodeReturn.action = PasscodeAction.Ok;
         this.Close();
      }
      private void cancelButton_Click(object sender, RoutedEventArgs e)
      {
         passcodeReturn.passcode = "";
         passcodeReturn.action = PasscodeAction.Cancel;
         this.Close();
      }

   }
}
