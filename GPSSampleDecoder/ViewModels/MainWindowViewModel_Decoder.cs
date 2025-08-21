using DocumentFormat.OpenXml.VariantTypes;
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Static;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;


namespace GPSSampleDecoder.ViewModels
{
   public partial class MainWindowViewModel : INotifyPropertyChanged
   {
      private int dbVersion = 318;
      private Configuration decryptedConfiguration = null;
      private List<Configuration> configurations = new List<Configuration>();
      private string rawJSON = null;
      private void DecodeCompleted(RunWorkerCompletedEventArgs e, string rawJSON, List<Configuration> configurations)

      // This event handler deals with the results of the
      // background operation.
      // private void decodeWorker_RunWorkerCompleted(
      //    object sender, RunWorkerCompletedEventArgs e)
      {
         // First, handle the case where an exception was thrown.
         if (e.Error != null)
         {
            decryptedConfiguration = null;
            //MessageBox.Show(StaticStrings.kNotValidConfig);
            MessageBox.Show(e.Error.ToString());
         }
         else if (e.Cancelled)
         {
            decryptedConfiguration = null;
            // Next, handle the case where the user canceled 
            // the operation.
            // Note that due to a race condition in 
            // the DoWork event handler, the Cancelled
            // flag may not have been set, even though
            // CancelAsync was called.
            //   resultLabel.Text = "Canceled";
         }
         else
         {
            DecodeComplete = true;
            decryptedConfiguration = e.Result as Configuration;
            this.rawJSON = rawJSON;
            this.configurations = configurations;

            HasConfigPath = true;
            // Finally, handle the case where the operation 
            // succeeded.
            // enable 

            if (decryptedConfiguration.dbVersion != dbVersion)
            {
               MessageBox.Show("The selected configuration is based on database version #" + decryptedConfiguration.dbVersion + " and is not compatable with this version of GPSSampleDecoder, which is based on version #" + dbVersion.ToString()+ ".", StaticStrings.kAppTitle, MessageBoxButton.OK, MessageBoxImage.Error);
               DecodeComplete = false;
               decryptedConfiguration = null;
               PercentDecoded = 0;
            }
            else
            {
               MessageBox.Show(StaticStrings.kDecodeComplete,
                        StaticStrings.kAppTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);

            }
         }
      }

      private void PercentDone(int percent)
      //private void decodeWorker_PercentDone(object sender, ProgressChangedEventArgs e)
      {
        // BackgroundWorker worker = sender as BackgroundWorker;
         PercentDecoded = percent;

      }

      private void DecodeError(string error)
      {
         DecodeComplete = false;
         SaveBrowseComplete = false;
         PercentDecoded = 0;
         PercentSaved = 0;
         MessageBox.Show(error,
            StaticStrings.kAppTitle, MessageBoxButton.OK, MessageBoxImage.Error);
         if (PathsToConfigurations.Count > 0)
         {
            HasConfigPath = true;
         }
      }
   }
}
