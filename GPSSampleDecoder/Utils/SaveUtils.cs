/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

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

        public SaveStateError SaveOutput(string path, string json, Configuration combinedConfiguration, List<Configuration> configurations, string imageFile, SaveState mode)
        {
            if (combinedConfiguration != null && combinedConfiguration.enumAreas != null && !String.IsNullOrEmpty(combinedConfiguration.name))
            {
                var configName = combinedConfiguration.name;

                // Write Excel File
                excelWriter.SaveExcel(combinedConfiguration, path);

                // Write JSON File
                using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(path, configName + ".json")))
                {
                    outputFile.WriteLine(json);
                }

                // Write GPX Files
                var bodyText = "";
                foreach (var enumArea in combinedConfiguration.enumAreas)
                {
                    // group breadcrumbs by groupId

                    if (enumArea.breadcrumbs.Count() > 1)
                    {
                        var groupId = enumArea.breadcrumbs.First().groupId;

                        List<Breadcrumb> breadcrumbs = new List<Breadcrumb>();
                        List<List<Breadcrumb>> breadcrumbGroups = new List<List<Breadcrumb>>();

                        foreach (var breadcrumb in enumArea.breadcrumbs)
                        {
                            if (breadcrumb.groupId == groupId)
                            {
                                breadcrumbs.Add(breadcrumb);

                                if (breadcrumb == enumArea.breadcrumbs.Last())
                                {
                                    breadcrumbGroups.Add(breadcrumbs);
                                }
                            }
                            else
                            {
                                groupId = breadcrumb.groupId;
                                breadcrumbGroups.Add(breadcrumbs);
                                breadcrumbs = new List<Breadcrumb>() { breadcrumb };
                            }
                        }

                        foreach (var breadcrumbGroup in breadcrumbGroups)
                        {
                            if (breadcrumbGroup.Count > 1)
                            {
                                bodyText += createTrk(enumArea.name, breadcrumbGroup, combinedConfiguration.timeZone);
                            }
                        }
                    }
                }

                if (bodyText.Length > 0)
                {
                    var fileText = "";

                    fileText += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\n";
                    fileText += "<gpx version=\"1.1\" creator=\"GPSSample\"" + "\n";
                    fileText += "xmlns=\"http://www.topografix.com/GPX/1/1\">" + "\n";
                    fileText += bodyText;
                    fileText += "</gpx>" + "\n";

                    string outpath = System.IO.Path.Combine(path, configName + ".gpx");
                    byte[] bytes = Encoding.UTF8.GetBytes(fileText);
                    File.WriteAllBytes(outpath, bytes);
                }
            }

            // Write Image Files
            if (imageFile != null)
            {
                try
                {
                    StreamReader sr = new StreamReader(imageFile);

                    string line = sr.ReadLine();

                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null) break;
                        Image image = JsonSerializer.Deserialize<Image>(line);
                        string outpath = System.IO.Path.Combine(path, image.locationUuid + ".jpg");
                        byte[] bytes = Convert.FromBase64String(image.data);
                        File.WriteAllBytes(outpath, bytes);
                    }

                    sr.Close();

                    Directory.Delete(Path.GetDirectoryName(imageFile), recursive: true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return SaveStateError.Success;

        }

        public string createTrk(String enumAreaName, List<Breadcrumb> breadcrumbs, int timeZoneOffset)
        {
            string gpxText = "";

            Breadcrumb first = breadcrumbs.First();

            gpxText += $"  <wpt lat=\"{first.latitude}\" lon=\"{first.longitude}\">" + "\n";
            gpxText += $"    <name>Start</name>" + "\n";
            gpxText += $"  </wpt>" + "\n";

            gpxText += $"  <trk>" + "\n";
            gpxText += $"    <name>{enumAreaName}-{first.enumTeamName}</name>" + "\n";
            gpxText += $"    <trkseg>" + "\n";

            foreach (var breadcrumb in breadcrumbs)
            {
                var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(breadcrumb.creationDate).ToOffset(TimeSpan.FromHours(timeZoneOffset)).ToString("yyyy-MM-dd'T'HH:mm:sszzz");
                gpxText += $"      <trkpt lat=\"{breadcrumb.latitude}\" lon=\"{breadcrumb.longitude}\">" + "\n";
                gpxText += $"        <time>{dateTime}</time>" + "\n";
                gpxText += $"      </trkpt>" + "\n";
            }

            gpxText += $"    </trkseg>" + "\n";
            gpxText += $"  </trk>" + "\n";

            Breadcrumb last = breadcrumbs.Last();

            gpxText += $"  <wpt lat=\"{last.latitude}\" lon=\"{last.longitude}\">" + "\n";
            gpxText += $"    <name>Finish</name>" + "\n";
            gpxText += $"  </wpt>" + "\n";

            return gpxText;
        }
    }
}
