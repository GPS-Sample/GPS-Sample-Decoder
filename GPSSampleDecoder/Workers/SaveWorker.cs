/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
using GPSSampleDecoder.Utils;
using System.ComponentModel;
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Delegates;
using GPSSampleDecoder.Static;
using System.Collections.Generic;

namespace GPSSampleDecoder.Workers
{
   internal class SaveData
   {

      public string outputPath { set; get; }
      public string JSON { set; get; }
      public Configuration combinedConfiguration = null;
      public List<Configuration> configurations { set; get; }
      public SaveState state { set; get; }
   }

	public class SaveWorker
	{
      private bool hasError = false;
      private string OutputPath { get; set; }
      private SaveUtils saveUtils;
      private string rawJSON;
      private Configuration decryptedConfiguration;
      private BackgroundWorker saveWorker;

      private static readonly Lazy<SaveWorker> lazy = new Lazy<SaveWorker>(() => new SaveWorker());

      
      public SavePercentDone PercentDone { get; set; }
      public SaveCompleted SaveCompleted { get; set; }
      public SaveError SaveError { get; set; }

      public static SaveWorker Instance
      {
         get
         {
            return lazy.Value;
         }
      }

      private SaveWorker()
      {

         saveUtils = SaveUtils.Instance;
         saveWorker = new BackgroundWorker();
         saveWorker.WorkerReportsProgress = true;
         saveWorker.WorkerSupportsCancellation = true;
         saveWorker.DoWork += saveWorker_DoWork;
         saveWorker.RunWorkerCompleted += saveWorker_RunWorkerCompleted;
         saveWorker.ProgressChanged += saveWorker_PercentDone;
      }

      public void StartSaving(string pathToFile, string rawJSON, Configuration combinedConfiguration, List<Configuration> configurations, SaveState saveState)
      {
         var data = new SaveData();
         data.outputPath = pathToFile;
         data.JSON = rawJSON;
         data.combinedConfiguration = combinedConfiguration;
         data.configurations = configurations;
         data.state = saveState;
         saveWorker.RunWorkerAsync(data);
      }

      private void saveWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorker worker = sender as BackgroundWorker;
         worker.ReportProgress(10);
         var saveData = e.Argument as SaveData;
         try
         {
            saveUtils.SaveOutput(saveData.outputPath, saveData.JSON, saveData.combinedConfiguration, saveData.configurations, saveData.state);
            worker.ReportProgress(100);

         }catch(Exception ex)
         {
            Console.WriteLine(ex.ToString());
            hasError = true;
            SaveError("Unable to save selcted file. ");
         }
      }

      private void saveWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         if(SaveCompleted != null && !hasError)
         {
            this.SaveCompleted(e);

         }


         if (hasError)
         {
            hasError = false;
         }
      }

      private void saveWorker_PercentDone(object sender, ProgressChangedEventArgs e)
      {
         this.PercentDone(e.ProgressPercentage);

      }
   }
}

