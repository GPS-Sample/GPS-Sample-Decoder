using GPSSampleDecoder.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GPSSampleDecoder.ViewModels
{
   public partial class MainWindowViewModel
   {
      
      private string outputPath;

      private void SaveCompleted(RunWorkerCompletedEventArgs e)
      {
         MessageBox.Show(StaticStrings.kSaveComplete,
         StaticStrings.kAppTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
      }

      private void SavePercentDone(int percentDone)
      {
         PercentSaved = percentDone;
      }
      private void SaveError(string error)
      {
         DecodeComplete = false;
         SaveBrowseComplete = false;
         PercentDecoded = 0;
         PercentSaved = 0;
         MessageBox.Show(error,
            StaticStrings.kAppTitle, MessageBoxButton.OK, MessageBoxImage.Error);
      }
   }

}
