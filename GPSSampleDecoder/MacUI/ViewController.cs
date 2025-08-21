using System;

using AppKit;
using Foundation;
using GPSSampleDecoder.Delegates;
using GPSSampleDecoder.MacUI;
using GPSSampleDecoder.Static;
using System.Collections.Generic;

using GPSSampleDecoder.Workers;

namespace GPSSampleDecoder
{
   public partial class ViewController : NSViewController
   {
      private int _percentDecoded = 0;
      private int _percentSaved = 0;
      private List<string> PathsToConfigurations = new List<String>();
      private string PathToOutput { get; set; }

      private DecodeWorker decodeWorker;
      private SaveWorker saveWorker;
      private SaveState _saveState = SaveState.Xls;


      public bool CreateExcel
      {
         get
         {
            return (_saveState == SaveState.Xls || _saveState == SaveState.CsvAndXls);
         }
         set
         {
            if (value)
            {
               if (_saveState == SaveState.Csv)
               {
                  _saveState = SaveState.CsvAndXls;
               }
               else
               {
                  _saveState = SaveState.Xls;
               }
            }
            else
            {
               if (_saveState == SaveState.CsvAndXls)
               {
                  _saveState = SaveState.Csv;
               }
               else
               {
                  _saveState = SaveState.Invalid;
               }
            }

         }
      }

      public bool CreateCSV
      {
         get
         {
            return (_saveState == SaveState.Csv || _saveState == SaveState.CsvAndXls);
         }
         set
         {
            if (value)
            {
               if (_saveState == SaveState.Xls)
               {
                  _saveState = SaveState.CsvAndXls;
               }
               else
               {
                  _saveState = SaveState.Csv;
               }
            }
            else
            {
               if (_saveState == SaveState.CsvAndXls)
               {
                  _saveState = SaveState.Xls;
               }
               else
               {
                  _saveState = SaveState.Invalid;
               }
            }

         }
      }

      private int PercentDecoded
      {
         get => _percentDecoded;
         set
         {
            _percentDecoded = value;
            decodeIndicator.DoubleValue = value;
         }
      }
      private int PercentSaved
      {
         get => _percentSaved;
         set
         {
            _percentSaved = value;
            saveIndicator.DoubleValue = value;
         }
      }
      public ViewController(IntPtr handle) : base(handle)
      {
         decodeWorker = DecodeWorker.Instance;
         decodeWorker.DecodeCompleted += DecodeCompleted;
         decodeWorker.PercentDone += DecodePercentDone;
         decodeWorker.DecodeError += DecodeError;

         DecodeComplete = false;

         saveWorker = SaveWorker.Instance;
         saveWorker.SaveCompleted += SaveCompleted;
         saveWorker.PercentDone += SavePercentDone;
         saveWorker.SaveError += SaveError;

      }

      public override void ViewDidLoad()
      {
         base.ViewDidLoad();

         // Do any additional setup after loading the view.
         // figure out dark mode
         string mode = NSUserDefaults.StandardUserDefaults.StringForKey("AppleInterfaceStyle");
         if (mode != null && mode.ToLower().Contains("dark"))
         {
            NSImage newicon = NSImage.ImageNamed("cdc_push pin_dark");
            titleImage.Image = newicon;
         }
         description.PreferredMaxLayoutWidth = this.View.Frame.Width - 36;
         description.StringValue = StaticStrings.kInstructions;

         outputDescription.PreferredMaxLayoutWidth = this.View.Frame.Width - 36;
         outputDescription.StringValue = StaticStrings.kOutputInstructions;
      }

      public override NSObject RepresentedObject
      {
         get
         {
            return base.RepresentedObject;
         }
         set
         {
            base.RepresentedObject = value;
            // Update the view, if already loaded.
         }
      }

      partial void configurationBrowseClicked(Foundation.NSObject sender)
      {
         resetElements();

         var dlg = NSOpenPanel.OpenPanel;
         dlg.CanChooseFiles = true;
			dlg.CanChooseDirectories = false;
			dlg.AllowsMultipleSelection = true;
			dlg.AllowedFileTypes = new string[] { "zip" };

         PathsToConfigurations.Clear();

         if (dlg.RunModal() == 1)
         {
            if (dlg.Urls.Length > 0)
            {
					foreach (NSUrl url in dlg.Urls)
					{
                  PathsToConfigurations.Add(url.Path);

						configurationPath.StringValue = url.Path;

						PathToOutput = url.Path.Replace(url.LastPathComponent, "");
						outputPath.StringValue = PathToOutput;

						saveButton.Enabled = true;
						decodeButton.Enabled = true;
						outputBrowseButton.Enabled = true;
					}				
            }
         }
      }

      partial void outputBrowseClicked(Foundation.NSObject sender)
      {
         saveButton.Enabled = false;
         saveIndicator.DoubleValue = 0.0;
         var dlg = NSOpenPanel.OpenPanel;
         dlg.CanChooseFiles = false;
         dlg.CanChooseDirectories = true;

         if (dlg.RunModal() == 1)
         {
            var url = dlg.Urls[0];

            if (url != null)
            {
               PathToOutput = url.Path;
               outputPath.StringValue = PathToOutput;
               saveButton.Enabled = true;
            }
         }
      }

      partial void decodeClicked(Foundation.NSObject sender)
      {
         
         if (PathsToConfigurations.Count > 0)
         {
            decodeButton.Enabled = false;
            decodeWorker.StartDecoding(PathsToConfigurations, passcode.StringValue);
         }
         
      }

      partial void saveClicked(Foundation.NSObject sender)
      {
         if ( !(string.IsNullOrEmpty( PathToOutput) || string.IsNullOrWhiteSpace(PathToOutput)) )
         {
            saveButton.Enabled = false;
            if(saveWorker != null)
            {
               saveWorker.StartSaving(PathToOutput, rawJSON, decryptedConfiguration, configurations, _saveState);
            }
            
         }
      }

      partial void csvChecked(NSObject sender)
      {
         NSButton csv = sender as NSButton;
         switch(csv.State)
         {
            case NSCellStateValue.On:
               CreateCSV = true;
               break;
            case NSCellStateValue.Off:
               CreateCSV = false;
               break;
         }
      }

      partial void xlsChecked(NSObject sender)
      {
         NSButton xls = sender as NSButton;
         switch (xls.State)
         {
            case NSCellStateValue.On:
               CreateExcel = true;
               break;
            case NSCellStateValue.Off:
               CreateExcel = false;
               break;
         }
      }

      public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
      {
         base.PrepareForSegue(segue, sender);

         Console.WriteLine("DO WE GET CALLED\n");
         // Take action based on the segue name
         switch (segue.Identifier)
         {
            case "DialogSegue":
               var dialog = segue.DestinationController as DialogViewController;
               
               // set up dialog
               if (sender == decodeButton)
               {
                  dialog.SetTitleAndContent("Decode", "Decoding Successful");
                  
               }
               if (sender == saveButton)
               {
                  dialog.SetTitleAndContent("Save", "Saving Successful");
                  
               }
               dialog.Title = "AMCKdsflj";//.DialogTitle = "MacDialog";
               //dialog.DialogDescription = "This is a sample dialog.";
               dialog.DialogAccepted += (s, e) => {
                  
                  try
                  {
                    // DismissViewController(dialog);
                  }
                  catch(Exception ex)
                  {

                  }
               };
               dialog.Presentor = this;
               break;

      
         }
      }

      private void resetElements()
      {
         this.errorMsg.StringValue = "";
         saveButton.Enabled = false;
         outputBrowseButton.Enabled = false;
         decodeButton.Enabled = false;
         decodeIndicator.DoubleValue = 0.0;
         saveIndicator.DoubleValue = 0.0;
      }
   }
}
