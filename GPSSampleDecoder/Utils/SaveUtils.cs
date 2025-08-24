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

      public SaveStateError SaveOutput(string path, string json, Configuration combinedConfiguration, List<Configuration> configurations, SaveState mode)
      {
         // based on mode, save either xls, csv or throw error
         var configName = "Config";

         if(combinedConfiguration != null && !String.IsNullOrEmpty(combinedConfiguration.name))
         {
            configName = combinedConfiguration.name;
         }

         if(mode == SaveState.Xls || mode == SaveState.CsvAndXls)
         {
            excelWriter.SaveExcel(combinedConfiguration, path);

         }

         using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, configName + JSON)))
         {
            //foreach (string line in lines)
            outputFile.WriteLine(json);
         }

         // get image data

         foreach( Configuration configuration in configurations)
         {
				Dictionary<string, byte[]> imageData = imageUtils.GetImagesFromConfiguration(configuration);
				foreach (var name in imageData.Keys)
				{
					string filename = name;
					if (filename.EndsWith("."))
					{
						filename = filename.Substring(0, filename.Length - 1);
					}
					string outpath = Path.Combine(path, filename + PNG);
					File.WriteAllBytes(outpath, imageData[name]);
				}
			}

			return SaveStateError.Success;

      }

   }
}
