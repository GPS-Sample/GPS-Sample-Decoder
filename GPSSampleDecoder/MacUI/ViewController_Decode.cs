/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using AppKit;
using Foundation;
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Delegates;
using GPSSampleDecoder.Static;

namespace GPSSampleDecoder
{
   public partial class ViewController : NSViewController
   {
      private bool _decodeComplete;
      private Configuration decryptedConfiguration = null;
      private List<Configuration> configurations = new List<Configuration>();
      private string rawJSON = null;

      public bool DecodeComplete
      {
         get => _decodeComplete;
         set
         {
            _decodeComplete = value;
         }
      }

      private void DecodeCompleted(RunWorkerCompletedEventArgs e, string rawJSON, List<Configuration>configurations)
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
            // show dialog

            //MessageBox.Show(e.Error.ToString());
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
            decodeButton.Enabled = true;
            outputBrowseButton.Enabled = true;

            //Console.WriteLine($"This is the raw JSON {this.rawJSON}");
            InvokeOnMainThread(() =>
            {
               try
               {
                  if (decryptedConfiguration.dbVersion != 318)
                  {
                     //DecodeComplete = false;
                     //outputBrowseButton.Enabled = false;
                     //decryptedConfiguration = null;
                     this.errorMsg.StringValue = "Database version mismatch. Expected version #318, got version #" + decryptedConfiguration.dbVersion;
							this.errorMsg.TextColor = NSColor.Red;
						}
						else
                  {
							this.errorMsg.StringValue = StaticStrings.kDecodeComplete;
							this.errorMsg.TextColor = NSColor.Green;
						}
						// this.PerformSegue("WindowSegue", this.decodeButton);
						//this.PerformSegue("DialogSegue", this.decodeButton);
					}
               catch (Exception ex)
               {
                  Console.WriteLine($"\n\n {ex}");
               }
            });
            //this.PerformSegue("DialogSegue", this);
            // Finally, handle the case where the operation 
            // succeeded.
            // enable 
            //MessageBox.Show("Decode Complete");
         }

      }

      private void DecodePercentDone(int percent)
      //private void decodeWorker_PercentDone(object sender, ProgressChangedEventArgs e)
      {
         // BackgroundWorker worker = sender as BackgroundWorker;
         PercentDecoded = percent;
       
      }

      private void DecodeError(string msg)
      {
         Console.WriteLine($"Error message {msg}");
         InvokeOnMainThread(() =>
         {
            try
            {
               errorMsg.StringValue = msg;
               errorMsg.TextColor = NSColor.Red;
               decodeButton.Enabled = true;
               decodeIndicator.DoubleValue = 0.0;
               saveIndicator.DoubleValue = 0.0;
               // resetElements();
            }
            catch (Exception ex)
            {
               Console.WriteLine($"the exception {ex.ToString()}");

            }
         });
      }
      
   }

}


