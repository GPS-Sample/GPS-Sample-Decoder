/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using DocumentFormat.OpenXml.Vml;
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

            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(path, configName + ".json")))
            {
                //foreach (string line in lines)
                outputFile.WriteLine(json);
            }

            if (imageList != null)
            {
                foreach (var image in imageList.images)
                {
                    string outpath = System.IO.Path.Combine(path, image.locationUuid + ".jpg");
                    byte[] bytes = Convert.FromBase64String(image.data);
                    File.WriteAllBytes(outpath, bytes);
                }
            }

            var trks = "";

            foreach (var enumArea in combinedConfiguration.enumAreas)
            {
                if (enumArea.breadcrumbs.Count() > 1)
                {
                    trks += createTrk(enumArea);
                }
            }

            if (trks.Length > 0)
            {
                var gpxText = "";

                gpxText += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\n";
                gpxText += "<gpx version=\"1.1\" creator=\"GPSSample\"" + "\n";
                gpxText += "xmlns=\"http://www.topografix.com/GPX/1/1\">" + "\n";
                gpxText += trks;
                gpxText += "</gpx>" + "\n";

                string outpath = System.IO.Path.Combine(path, configName + ".gpx");
                byte[] bytes = Encoding.UTF8.GetBytes(gpxText);
                File.WriteAllBytes(outpath, bytes);
            }

            return SaveStateError.Success;

        }

        public string createTrk(EnumArea enumArea) 
        {
            string gpxText = "";

            Breadcrumb first = enumArea.breadcrumbs.First();

            gpxText += $"  <wpt lat=\"{first.latitude}\" lon=\"{first.longitude}\">" + "\n";
            gpxText += $"    <name>Start ({enumArea.name})</name>" + "\n";
            gpxText += $"  </wpt>" + "\n";

            gpxText += $"  <trk>" + "\n";
            gpxText += $"    <trkseg>" + "\n";

            foreach (var breadcrumb in enumArea.breadcrumbs)
            {
                var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(breadcrumb.creationDate).ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                gpxText += $"      <trkpt lat=\"{breadcrumb.latitude}\" lon=\"{breadcrumb.longitude}\">" + "\n";
                gpxText += $"        <time>{dateTime}</time>" + "\n";
                gpxText += $"      </trkpt>" + "\n";
            }

            gpxText += $"    </trkseg>" + "\n";
            gpxText += $"  </trk>" + "\n";

            Breadcrumb last = enumArea.breadcrumbs.Last();

            gpxText += $"  <wpt lat=\"{last.latitude}\" lon=\"{last.longitude}\">" + "\n";
            gpxText += $"    <name>Finish ({enumArea.name})</name>" + "\n";
            gpxText += $"  </wpt>" + "\n";

            return gpxText;
        }
    }
}
