/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Static;
using GPSSampleDecoder.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;
using GPSSampleDecoder.Delegates;
using GPSSampleDecoder.Workers;
using GPSSampleDecoder.Views;

namespace GPSSampleDecoder.ViewModels
{

   public partial class MainWindowViewModel : INotifyPropertyChanged
   {
     
      private EncryptionUtil encryptionUtil;
      private SaveUtils saveUtils;
      //private const string kNoFileSelected = "No encrypted Configuration selected.\nPlease select a file.";
      private DecodeWorker decodeWorker;
      private SaveWorker saveWorker;

      private int _percentDecoded = 0;
      private int _percentSaved = 0;
      //private Visibility _decodeHidden = Visibility.Collapsed;
      private Visibility _decodeHidden = Visibility.Visible;
      private bool _close = false;
      private bool _decodeComplete = false;
      private bool _saveBrowseComplete = false;
      private string _pathToConfiguration = "";
      private bool _hasConfigPath = false;
      private string _pathToOutput = "";
      private SaveState _saveState = SaveState.Xls;
      public string Instructions
      {
         get => StaticStrings.kInstructions;
      }

      public string OutputInstructions
      {
         get => StaticStrings.kOutputInstructions;
      }

      public List<string> PathsToConfigurations = new List<string>();
      
      public void setPathVars()
      { 
        DecodeComplete = false;
        PercentDecoded = 0;
        PercentSaved = 0;
        PathToOutput = "";
        SaveBrowseComplete = false;
        HasConfigPath = true;
      }

      public string PathToOutput
      {
         get => _pathToOutput;
         set
         {
            _pathToOutput = value;
            if(!String.IsNullOrEmpty( _pathToOutput)) 
            {
               SaveBrowseComplete = true;
               PercentSaved = 0;
            }            
            OnPropertyChange(nameof(PathToOutput));
         }
         
      }

      public string Error { get; set; } = "";
      public bool CreateExcel 
      { 
         get
         {
            return (_saveState == SaveState.Xls || _saveState == SaveState.CsvAndXls);
         }
         set
         {
            if(value)
            {
               if(_saveState == SaveState.Csv)
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
      public bool CreateCSV {
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

      public bool Close
      {
         get => _close;
         set
         {
            _close = value;
            if (_close)
            {
               CloseAction(this);
            }
         }
      }
      public int PercentDecoded
      {
         get => _percentDecoded;
         set
         {
            _percentDecoded = value;
            OnPropertyChange(nameof(PercentDecoded));
         }
      }

      public int PercentSaved
      {
         get => _percentSaved;
         set
         {
            _percentSaved = value;
            OnPropertyChange(nameof(PercentSaved));
         }
      }

      public Visibility DecodeVisibilty
      {
         get => _decodeHidden;
         set
         {
            _decodeHidden = value;
            OnPropertyChange(nameof(DecodeVisibilty));
         }
      }

      public bool DecodeComplete
      {
         get => _decodeComplete;
         set
         {
            _decodeComplete = value;
            OnPropertyChange(nameof(DecodeComplete));
         }
      }

      public bool SaveBrowseComplete
      {
         get => _saveBrowseComplete;
         set
         {
            _saveBrowseComplete = value;
            OnPropertyChange(nameof(SaveBrowseComplete));
         }
      }

      public bool HasConfigPath
      {
         get => _hasConfigPath;
         set
         {
            _hasConfigPath = value;
            OnPropertyChange(nameof(HasConfigPath));
         }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public MainWindowViewModel()
      {
         DecodeCommand = new RelayCommand(o => DecodeAction("DecodeButton"));
         CloseCommand = new RelayCommand(o => CloseAction("DecodeButton"));
         SaveCommand = new RelayCommand(o => SaveAction("SaveButton"));
         encryptionUtil = EncryptionUtil.Instance;
         saveUtils = SaveUtils.Instance;

         decodeWorker = DecodeWorker.Instance;
         decodeWorker.PercentDone += this.PercentDone;
         decodeWorker.DecodeCompleted += DecodeCompleted;
         decodeWorker.DecodeError += DecodeError;

         saveWorker = SaveWorker.Instance;
         saveWorker.SaveCompleted += SaveCompleted;
         saveWorker.PercentDone += SavePercentDone;
         saveWorker.SaveError += SaveError;

         //saveWorker = new BackgroundWorker();
         //saveWorker.WorkerReportsProgress = true;
         //saveWorker.WorkerSupportsCancellation = true;
         //saveWorker.DoWork += saveWorker_DoWork;
         //saveWorker.RunWorkerCompleted += saveWorker_RunWorkerCompleted;
         //saveWorker.ProgressChanged += saveWorker_PercentDone;
      }



      protected void OnPropertyChange(string propertyName)
      {
         if (PropertyChanged != null)
         {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
         }
      }


      public ICommand DecodeCommand { get; set; }
      public ICommand CloseCommand { get; set; }
      public ICommand SaveCommand { get; set; }

      private void CloseAction(object sender)
      {
         var Result = MessageBox.Show(StaticStrings.kExitString, 
            StaticStrings.kAppTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);

         if (Result == MessageBoxResult.Yes)
         {
            Environment.Exit(0);
         }
            
      }

      private void DecodeAction(object sender)
      {
         if (PathsToConfigurations.Count > 0)
         {
            HasConfigPath = false;
            PasswordDialog dialog = new PasswordDialog();
            PasscodeReturn val = dialog.ShowPasswordDialog();
            
            if(val.action == PasscodeAction.Ok)
            {
              // MessageBox.Show(val.passcode);
               decodeWorker.StartDecoding(PathsToConfigurations, val.passcode);
            }else
            {
               // just double check
               if(PathsToConfigurations.Count > 0)
               {
                  HasConfigPath = true;
               }
               
            }
            
         }
         else
         {
            MessageBox.Show(StaticStrings.kNoFileSelected,
             StaticStrings.kAppTitle, MessageBoxButton.OK, MessageBoxImage.Error);
         }
      }

      private void SaveAction(object sender)
      {

         if (PathToOutput != null && PathToOutput != "")
         {
            saveWorker.StartSaving(PathToOutput, rawJSON, decryptedConfiguration, configurations, _saveState);
         }
         else
         {
            MessageBox.Show(StaticStrings.kNoDirectorySelected,
               StaticStrings.kAppTitle, MessageBoxButton.OK, MessageBoxImage.Error);
         }
      }

   }

   public class RelayCommand : ICommand
   {
      private readonly Action<object> _execute;
      private readonly Predicate<object> _canExecute;

      public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
      {
         if (execute == null) throw new ArgumentNullException("execute");

         _execute = execute;
         _canExecute = canExecute;
      }

      public bool CanExecute(object parameter)
      {
         return _canExecute == null || _canExecute(parameter);
      }

      public event EventHandler CanExecuteChanged
      {
         add { CommandManager.RequerySuggested += value; }
         remove { CommandManager.RequerySuggested -= value; }
      }

      public void Execute(object parameter)
      {
         _execute(parameter ?? "<N/A>");
      }

   }
}
