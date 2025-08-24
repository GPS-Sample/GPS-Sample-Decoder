/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
using System.ComponentModel;
using AppKit;
using GPSSampleDecoder.Static;

namespace GPSSampleDecoder
{
   public partial class ViewController : NSViewController
   {
      public void SavePercentDone(int percent)
      {
         PercentSaved = percent;
      }

      public void SaveCompleted(RunWorkerCompletedEventArgs e)
      {
         try
         {
            this.errorMsg.StringValue = StaticStrings.kSaveComplete;
            this.errorMsg.TextColor = NSColor.Green;
            //this.PerformSegue("DialogSegue", this.saveButton);
         }
         catch (Exception ex)
         {
            Console.WriteLine($"\n\n {ex}");
         }
         saveButton.Enabled = true;
      }

      private void SaveError(string msg)
      {
         InvokeOnMainThread(() =>
         {
            try
            {
               errorMsg.StringValue = msg;
               errorMsg.TextColor = NSColor.Red;
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

