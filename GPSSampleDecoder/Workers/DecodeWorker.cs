/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using GPSSampleDecoder.Delegates;
using System.IO.Compression;
using Path = System.IO.Path;

namespace GPSSampleDecoder.Workers
{
    public class DecodeWorker
    {
        private bool hasError = false;
        private ImageList imageList = null;
        private Configuration decryptedConfiguration = null;
        private List<Configuration> configurations = new List<Configuration>();
        private string rawJSON = null;

        private EncryptionUtil encryptionUtil;
        private int _percentDecoded = 0;

        private bool _decodeComplete = false;
        private List<string> _pathsToConfigurations = new List<string>();
        private bool _hasConfigPath = false;
        private string _passcode = "";

        private BackgroundWorker decodeWorker;

        public DecodePercentDone PercentDone { get; set; }
        public DecodeCompleted DecodeCompleted { get; set; }
        public DecodeError DecodeError { get; set; }

        public int PercentDecoded
        {
            get => _percentDecoded;
            set
            {
                _percentDecoded = value;
            }
        }

        public bool HasConfigPath
        {
            get => _hasConfigPath;
            set
            {
                _hasConfigPath = value;
            }
        }

        public bool DecodeComplete
        {
            get => _decodeComplete;
            set
            {
                _decodeComplete = value;
            }
        }

        public List<string> PathsToConfigurations
        {
            get => _pathsToConfigurations;
            set
            {
                _pathsToConfigurations = value;
                DecodeComplete = false;
                PercentDecoded = 0;
                HasConfigPath = true;
            }
        }

        private static readonly Lazy<DecodeWorker> lazy = new Lazy<DecodeWorker>(() => new DecodeWorker());
        public static DecodeWorker Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private DecodeWorker()
        {
            encryptionUtil = EncryptionUtil.Instance;
            decodeWorker = new BackgroundWorker();
            decodeWorker.WorkerReportsProgress = true;
            decodeWorker.WorkerSupportsCancellation = true;
            decodeWorker.DoWork += decodeWorker_DoWork;
            decodeWorker.RunWorkerCompleted += decodeWorker_RunWorkerCompleted;
            decodeWorker.ProgressChanged += decodeWorker_PercentDone;
        }

        public void StartDecoding(List<string> pathsToFiles, string passcode)
        {
            _passcode = passcode;
            imageList = null;
            configurations.Clear();
            decryptedConfiguration = null;
            PathsToConfigurations = pathsToFiles;
            decodeWorker.RunWorkerAsync();
        }

        private void decodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;

                foreach (string pathToConfiguration in PathsToConfigurations)
                {
                    worker.ReportProgress(10);

                    string directoryPath = Path.GetDirectoryName(pathToConfiguration);

                    string configFileName = directoryPath + "/" + Path.GetFileNameWithoutExtension(pathToConfiguration) + ".json";
                    string imageFileName = directoryPath + "/" + Path.GetFileNameWithoutExtension(pathToConfiguration) + "-img.json";

                    if (File.Exists(configFileName))
                    {
                        File.Delete(configFileName);
                    }

                    if (File.Exists(imageFileName))
                    {
                        File.Delete(imageFileName);
                    }

                    ZipFile.ExtractToDirectory(pathToConfiguration, directoryPath);

                    decodeConfig(e, configFileName);

                    worker.ReportProgress(50);

                    decodeImages(e, imageFileName, directoryPath);
                    worker.ReportProgress(100);

                    if (File.Exists(configFileName))
                    {
                        File.Delete(configFileName);
                    }

                    if (File.Exists(imageFileName))
                    {
                        File.Delete(imageFileName);
                    }
                }

                worker.ReportProgress(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                hasError = true;
                DecodeError("Unable to decode selected file. \n Ensure it is an compressed zip file\n");
            }
        }

        private void decodeConfig(DoWorkEventArgs e, string pathToConfiguration)
        {
            try
            {
                StreamReader sr = new StreamReader(pathToConfiguration);

                string encrypted = sr.ReadToEnd();

                sr.Close();

                var decryptedFile = encryptionUtil.Decrypt(encrypted, _passcode);

                Configuration config = JsonSerializer.Deserialize<Configuration>(decryptedFile);

                configurations.Add(config);

                if (decryptedConfiguration == null)
                {
                    decryptedConfiguration = config;
                }
                else
                {
                    decryptedConfiguration.merge(config);
                }

                e.Result = decryptedConfiguration;

                rawJSON = JsonSerializer.Serialize(decryptedConfiguration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                hasError = true;
                DecodeError("Unable to decode selected config file. \n Ensure it is an encrypted JSON file\n" +
                   "Ensure you typed the passcode used to encrypt the JSON.");
            }
        }

        private void decodeImages(DoWorkEventArgs e, string pathToImageList, string directoryPath)
        {
            try
            {
                StreamReader sr = null;

                try
                {
                    sr = new StreamReader(pathToImageList);
                }
                catch (FileNotFoundException ex)
                {
                    return;
                }

                string encrypted = sr.ReadToEnd();

                sr.Close();

                var decryptedFile = encryptionUtil.Decrypt(encrypted, _passcode);

                imageList = JsonSerializer.Deserialize<ImageList>(decryptedFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                hasError = true;
                DecodeError("Unable to decode selected image file. \n Ensure it is an encrypted JSON file\n" +
                    "Ensure you typed the passcode used to encrypt the JSON.");
            }
        }

        // This event handler deals with the results of the
        // background operation.
        private void decodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (DecodeCompleted != null && !hasError)
            {
                DecodeCompleted(e, rawJSON, configurations, imageList);
            }
            if (hasError)
            {
                hasError = false;
            }
        }

        private void decodeWorker_PercentDone(object sender, ProgressChangedEventArgs e)
        {
            PercentDecoded = e.ProgressPercentage;
            this.PercentDone(PercentDecoded);

        }
    }
}

