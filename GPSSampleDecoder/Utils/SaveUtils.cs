/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Static;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.Utils
{
    public class SaveUtils
    {
        private const string JSON = ".json";
        private const string PNG = ".png";
        private ExcelWriter excelWriter = new ExcelWriter();
        private CSVWriter csvWriter = new CSVWriter();
        private ImageUtils imageUtils = new ImageUtils();
        private SaveUtils()
        {

        }
        private static readonly Lazy<SaveUtils> lazy = new Lazy<SaveUtils>(() => new SaveUtils());
        public static SaveUtils Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public SaveStateError SaveOutput(string path, string json, Configuration combinedConfiguration, List<Configuration> configurations, ImageList imageList, SaveState mode)
        {
            // based on mode, save either xls, csv or throw error
            var configName = "Config";

            if (combinedConfiguration != null && !String.IsNullOrEmpty(combinedConfiguration.name))
            {
                configName = combinedConfiguration.name;
            }

            if (mode == SaveState.Xls || mode == SaveState.CsvAndXls)
            {
                excelWriter.SaveExcel(combinedConfiguration, path);

            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, configName + JSON)))
            {
                //foreach (string line in lines)
                outputFile.WriteLine(json);
            }

            if (imageList != null)
            {
                foreach (var image in imageList.images)
                {
                    string outpath = Path.Combine(path, image.uuid + ".JPG");
                    byte[] bytes = Convert.FromBase64String(image.data);
                    File.WriteAllBytes(outpath, bytes);
                }
            }

            return SaveStateError.Success;

        }

    }
}
