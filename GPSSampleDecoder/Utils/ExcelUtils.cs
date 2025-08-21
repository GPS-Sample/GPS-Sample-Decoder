using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;
using GPSSampleDecoder.DataObjects;
using System.Data;
using DocumentFormat.OpenXml.Office2016.Excel;
using GPSSampleDecoder.Static;
using System.Globalization;
using System.Text.Json;

namespace GPSSampleDecoder.Utils
{
   public enum TimeFormat
   {
      DATE,
      TIME,
      DATETIME
   }

   public class ExcelUtils
   {

      public class TestModel
      {
         public int TestId { get; set; }
         public string TestName { get; set; }
         public string TestDesc { get; set; }
         public DateTime TestDate { get; set; }
      }
      public class TestModelList
      {
         public List<TestModel> testData { get; set; }
      }

      public ExcelUtils()
      {
      }

      public void ProcessExcel(Configuration config, string outputPath)
      {

         // p.CreateExcelFile(tmList, "d:\\");
         CreateExcel(outputPath, config);
         //test2(outputPath);
         //BigTest(outputPath);
      }

      private void CreateExcel(string outputPath, Configuration data)
      {
         string fileFullname = Path.Combine(outputPath, $"{data.name}.xlsx");
         WorkbookPart wBookPart = null;
         // DataSet tableSet = getDataSet(sourceTable);//getDataSet is my local function which is used to split a datatable into some datatable based on limited row I've declared.
         using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(fileFullname, SpreadsheetDocumentType.Workbook))
         {
            wBookPart = spreadsheetDoc.AddWorkbookPart();
            wBookPart.Workbook = new Workbook();
            uint sheetId = 1;
            spreadsheetDoc.WorkbookPart.Workbook.Sheets = new Sheets();
            Sheets sheets = spreadsheetDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            sheets.Append(createConfigurationSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(createEnumerationAreaSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(createEnumerationLocationsSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(creatStudiesSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(creatRulesSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(creatFiltersSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(createCollectionTeamsSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(createEnumerationItemsSheet(wBookPart, spreadsheetDoc, sheetId, data));
            sheetId += 1;
            sheets.Append(createEnumerationBlockItemsSheet(wBookPart, spreadsheetDoc, sheetId, data));
				sheetId += 1;
				sheets.Append(createSummarySheet(wBookPart, spreadsheetDoc, sheetId, data));
			}
		}

      private string findEnumName(Configuration data, string uuid)
      {
         string name = "";
         foreach (var enumAra in data.enumAreas)
         {
            if (enumAra.uuid == uuid)
            {
               name = enumAra.name;
               break;
            }
         }
         return name;
      }

      private string findEnumUUID(Configuration data, string uuid)
      {
         foreach (var enumArea in data.enumAreas)
         {
            if (enumArea.uuid == uuid)
            {
               return enumArea.uuid;
            }
         }
         return "";
      }

      private List<string> getValueFromFieldData(DataObjects.FieldData data, List<Study> studies)
      {
         List<string> values = new List<string>();

         string val = "";
         bool needToCheck = false;
         switch (data.type)
         {
            case ("Text"):
               {
                 // values.Clear();
                  values.Add(data.textValue);
                  return values;
               }
            case ("Date"):
               {
                  string dateFormatted = "";
                  if(data.dateValue != null)
                  {
                     double timeVal = (double)data.dateValue;
                     //TimeSpan time = TimeSpan.FromMilliseconds(timeVal);
                     //DateTime startdate = new DateTime(1970, 1, 1) + time;
                     dateFormatted = ConvertTimeBasedOnTimezone(timeVal, TimeFormat.DATE); //startdate.ToString("MM/dd/yyyy");

                  }
               //   values.Clear();
                  values.Add(dateFormatted);

                  return values;
               }
            case ("Number"):
               {
               //   values.Clear();
                  values.Add($"{data.numberValue}");

                  return values;
               }
            case ("Dropdown"):
               {
               //   values.Clear();
                  if (data.dropdownIndex != null)
                  {
                     int index = (int)data.dropdownIndex;
                     values.Add(data.fieldDataOptions[index].name);
                  }
                  
                  return values;

               }
            case ("None"):
               needToCheck = true;
               break;
         }


         if (needToCheck)
         {
            DataObjects.Field field = getField(data.fieldUuid, studies);

				// find checkbox box
				if (field != null && field.type.ToLower().Contains("checkbox"))
            {
               //    values.Clear();
               string selected = "";
               foreach(var fdo in data.fieldDataOptions)
               {
                  if(fdo.value)
                  {
                     selected += fdo.name + ", ";
                  }
               }
               if(selected.EndsWith(", "))
               {
                  selected = selected.Remove(selected.Length - 2);
               }
               values.Add(selected);
               return values;
            }
            if (!string.IsNullOrEmpty(data.textValue))
            {
             //  values.Clear();
               values.Add(data.textValue);
               return values;
            }
            else if (data.numberValue != null)
            {
            //   values.Clear();
               values.Add($"{data.numberValue}");

               return values;
            }
            else if (data.dateValue != null)
            {
               double timeVal = (double)data.dateValue;
               //TimeSpan time = TimeSpan.FromMilliseconds(timeVal);
               //DateTime startdate = new DateTime(1970, 1, 1) + time;

               string dateFormatted = ConvertTimeBasedOnTimezone((double)timeVal, TimeFormat.DATETIME);//startdate.ToString("MM/dd/yyyy HH:mm:ss");
               values.Add(dateFormatted);

               return values;
            }
            else if (data.dropdownIndex != null)
            {
               int index = (int)data.dropdownIndex;
               if (index >= 0 && index < data.fieldDataOptions.Count)
               {
       //           values.Clear();
                  values.Add(data.fieldDataOptions[index].name);
               
                  return values;
               }
            }
         }

     //    values.Clear();
         return values;
      }

      int addFieldDataList(Dictionary<string, int> headerFields, Dictionary<int, List<string>> sorted, EnumerationItem enumItem, List<Study> studies )
      {
         // Dictionary<int, List<string>> sorted = new Dictionary<int, List<string>>();
         foreach (var val in headerFields.Values)
         {
            sorted.Add(val, new List<string>());
         }

         foreach (var fd in enumItem.fieldDataList)
         {
            if (headerFields.Keys.Contains(fd.fieldUuid))
            {
               int index = headerFields[fd.fieldUuid];
               var test = getValueFromFieldData(fd,studies);
               foreach (var strVal in test)
               {
                  if(test.Count > 0)
                  {
                     sorted[index].Add(strVal);
                  }
               }
            }
         }
         
         // now we find the largest list.  some of these can be null, or one enum item is missing something..
         int largest = 0;
         foreach (var list in sorted.Values)
         {
            if (largest < list.Count)
            {
               largest = list.Count;
            }
         }

         return largest == 0 ? 1 : largest;
      }

      private Sheet createEnumerationBlockItemsSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = $"Block Questions" };
         // sheets.Append(sheet);

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);

         Row headerRow = new Row();
         headerRow.Append(CreateCell("UUID"));
         Dictionary<string, int> headerFields = new Dictionary<string, int>();
         int row = 0;
         foreach (Study study in data.studies)
         {
            foreach (var field in study.fields)
            {
               if (field.fields != null)
               {
						foreach (var childField in field.fields)
						{
							var fieldName = field.index.ToString() + "." + childField.index.ToString() + ". " + childField.name;
							headerRow.Append(CreateCell(fieldName));
							row = headerRow.Count() - 1;
							headerFields.Add(childField.uuid, row);
						}
					}
				}
         }

         sheetData.AppendChild(headerRow);

         foreach (var enumArea in data.enumAreas)
         {
				foreach (var location in enumArea.locations)
            {
					foreach (var enumItem in location.enumerationItems)
					{
						// filter out block field containers that have no entries
						bool shouldSkip = false;

                  foreach (var fd in enumItem.fieldDataList)
                  {
                     DataObjects.Field field = getField(fd.fieldUuid, data.studies);
                     if (field != null && field.fields?.Count > 0) // this is a block field container
                     {
                        if (fd.numberValue == null || fd.numberValue == 0)
                        {
                           shouldSkip = true;
                        }
                     }
                  }

                  if (shouldSkip)
						{
							continue;
						}

						Dictionary<int, List<string>> sorted = new Dictionary<int, List<string>>();

						// largest is the number of rows we have to add
						int largest = addFieldDataList(headerFields, sorted, enumItem, data.studies);

						for (int i = 0; i < largest; i++)
						{
							Row eiRow = new Row();
							eiRow.Append(CreateCell(enumItem.uuid));
							// add the correct number of cells 
							for (int k = 0; k < sorted.Count(); k++)
							{
								eiRow.Append(CreateCell(""));
							}
							foreach (var key in sorted.Keys)
							{
								if (i < sorted[key].Count)
								{
									eiRow.InsertAt(CreateCell(sorted[key][i]), key);
								}

							}
							sheetData.AppendChild(eiRow);
						}
					}
				}
         }

         return sheet;
      }

      private Sheet createEnumerationLocationsSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = $"Enumeration Landmarks" };
         // sheets.Append(sheet);

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);

         Row headerRow = new Row();

			headerRow.Append(CreateCell("Location UUID"));
			headerRow.Append(CreateCell("Location"));
         headerRow.Append(CreateCell("Type"));
         headerRow.Append(CreateCell("Latitude"));
         headerRow.Append(CreateCell("Longitude"));
         headerRow.Append(CreateCell("Altitude"));

            // enumeration item

            sheetData.AppendChild(headerRow);

         foreach (var ea in data.enumAreas)
         {
            foreach(var location in ea.locations)
            {
               if(location.isLandmark)
               {

                  Row landmarkRow = new Row();
						landmarkRow.Append(CreateCell(location.uuid));
						landmarkRow.Append(CreateCell(location.description));
                  landmarkRow.Append(CreateCell(location.type));
                  landmarkRow.Append(CreateCell($"{location.latitude}"));
                  landmarkRow.Append(CreateCell($"{location.longitude}"));
                  landmarkRow.Append(CreateCell($"{location.altitude}"));
                  sheetData.AppendChild(landmarkRow);
               }
               
            }

         }

         return sheet;
      }


      private Sheet createEnumerationItemsSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = $"Enumeration Items" };
         // sheets.Append(sheet);

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);

         Row headerRow = new Row();
         headerRow.Append(CreateCell("Enumeration Item UUID"));
         headerRow.Append(CreateCell("Location UUID"));
         headerRow.Append(CreateCell("Enumeration Area Name"));
         headerRow.Append(CreateCell("SubAddress"));

         int numProperties = 0;

         foreach (var enumArea in data.enumAreas)
         {
				if (enumArea.locations.Count > 0)
				{
					DataObjects.Location location = enumArea.locations[0];

					if (location.properties.Length > 0)
					{
						var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(location.properties);

						foreach (var item in dictionary)
						{
							headerRow.Append(CreateCell(item.Key));
						}

                  numProperties = dictionary.Count;
                  break;
					}
				}
			}

			headerRow.Append(CreateCell("GPS Accuracy"));
         headerRow.Append(CreateCell("Latitude"));
         headerRow.Append(CreateCell("Longitude"));
         headerRow.Append(CreateCell("Altitude"));
         headerRow.Append(CreateCell("Multi Household"));
         headerRow.Append(CreateCell("Enumerator Name"));
         headerRow.Append(CreateCell("Collector Name"));
         headerRow.Append(CreateCell("Enumerated"));
         headerRow.Append(CreateCell("Date Enumerated"));
         headerRow.Append(CreateCell("Time Enumerated"));
         headerRow.Append(CreateCell("Incomplete Enumeration Reason"));
         headerRow.Append(CreateCell("Enumeration Notes"));
         headerRow.Append(CreateCell("Meets Selection Criteria"));
         headerRow.Append(CreateCell("Probability of Selection"));
         headerRow.Append(CreateCell("Sampled"));
         headerRow.Append(CreateCell("Surveyed"));
         headerRow.Append(CreateCell("Date Surveyed"));
         headerRow.Append(CreateCell("Time Surveyed"));
         headerRow.Append(CreateCell("Incomplete Survey Reason"));
         headerRow.Append(CreateCell("Survey Notes"));

         Dictionary<string, int> headerFields = new Dictionary<string, int>();
         int row = 0;
         foreach (Study study in data.studies)
         {
            foreach (var field in study.fields)
            {
					headerRow.Append(CreateCell(field.index.ToString() + ". " + field.name));
					row = headerRow.Count() - 1;
					headerFields.Add(field.uuid, row);
            }
         }

         // enumeration item

         sheetData.AppendChild(headerRow);

         foreach (var enumArea in data.enumAreas)
         {
            float totalSampled = 0;
            float totalEligible = 0;

            foreach (var location in enumArea.locations)
            {
               foreach (var enumItem in location.enumerationItems)
               {
                  if (enumItem.samplingState == "Sampled")
                  {
                    totalSampled += 1;
                  }
                  if (enumItem.enumerationEligibleForSampling)
                  {
                     totalEligible += 1;
                  }
               }
            }

            foreach (var location in enumArea.locations)
            {
               string test = location.description;

               foreach (var enumItem in location.enumerationItems)
               {
                  Dictionary<int, List<string>> sorted = new Dictionary<int, List<string>>();

                  int largest = addFieldDataList(headerFields, sorted, enumItem, data.studies);
                  // largest is the number of rows we have to add

                  for (int i = 0; i < largest; i++)
                  {
                     Row eiRow = new Row();

                     eiRow.Append(CreateCell(enumItem.uuid));
                     eiRow.Append(CreateCell(location.uuid));
                     eiRow.Append(CreateCell(enumArea.name));
                     eiRow.Append(CreateCell(enumItem.subAddress));

                     if (location.properties.Length > 0)
                     {
								var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(location.properties);

								foreach (var item in dictionary)
								{
									eiRow.Append(CreateCell(item.Value));
								}
							}
                     else
                     {
                        for (i=0;i<numProperties;i++)
                        {
									eiRow.Append(CreateCell(""));
								}
							}

							eiRow.Append(CreateCell($"{location.gpsAccuracy}"));
                     eiRow.Append(CreateCell($"{location.latitude}"));
                     eiRow.Append(CreateCell($"{location.longitude}"));
                     eiRow.Append(CreateCell($"{location.altitude}"));

                     if (location.enumerationItems.Count > 1)
                     {
                        eiRow.Append(CreateCell("Yes"));
                     }
                     else
                     {
                        eiRow.Append(CreateCell("No"));
                     }
                     eiRow.Append(CreateCell(enumItem.enumeratorName));
                     eiRow.Append(CreateCell(enumItem.collectorName));

                     if (enumItem.enumerationState == "Enumerated")
                        eiRow.Append(CreateCell("Yes"));
                     else
                        eiRow.Append(CreateCell("No"));

                     if (enumItem.enumerationDate > 1)
                     {
                        double unixTime = enumItem.enumerationDate + location.timeZone * 60 * 60 * 1000;
                        string date = ConvertTimeBasedOnTimezone(unixTime, TimeFormat.DATE);
                        string time = ConvertTimeBasedOnTimezone(unixTime, TimeFormat.TIME);
                        eiRow.Append(CreateCell(date));
                        eiRow.Append(CreateCell(time));
                     }
                     else
                     {
                        eiRow.Append(CreateCell(""));
                        eiRow.Append(CreateCell(""));
                     }

                     eiRow.Append(CreateCell(enumItem.enumerationIncompleteReason));
                     eiRow.Append(CreateCell(enumItem.enumerationNotes));

                     if (enumItem.enumerationEligibleForSampling)
                        eiRow.Append(CreateCell("Yes"));
                     else
                        eiRow.Append(CreateCell("No"));

                     double pos = 0.0;

                     if (enumItem.enumerationEligibleForSampling)
                     {
                        if (totalEligible > 0 && totalSampled > 0)
                        {
                           if (totalEligible <= totalSampled)
                           {
                              pos = 1.0;
                           }
                           else
                           {
                              pos = totalSampled / totalEligible;
                           }
                        }
                     }

                     eiRow.Append(CreateCell(String.Format("{0:0.#####}", pos)));

                     if (enumItem.samplingState == "Sampled")
                        eiRow.Append(CreateCell("Yes"));
                     else
                        eiRow.Append(CreateCell("No"));

                     if (enumItem.collectionState == "Complete")
                        eiRow.Append(CreateCell("Yes"));
                     else
                        eiRow.Append(CreateCell("No"));

                     if (enumItem.collectionDate > 1)
                     {
                        double unixTime = enumItem.collectionDate + location.timeZone * 60 * 60 * 1000;
                        string date = ConvertTimeBasedOnTimezone(unixTime, TimeFormat.DATE);
                        string time = ConvertTimeBasedOnTimezone(unixTime, TimeFormat.TIME);
                        eiRow.Append(CreateCell(date));
                        eiRow.Append(CreateCell(time));
                     }
                     else
                     {
                        eiRow.Append(CreateCell(""));
                        eiRow.Append(CreateCell(""));
                     }

                     eiRow.Append(CreateCell(enumItem.collectionIncompleteReason));
                     eiRow.Append(CreateCell(enumItem.collectionNotes));

                     // add the correct number of cells
                     for (int k = 0; k < sorted.Count(); k++)
                     {
                        eiRow.Append(CreateCell(""));
                     }

                     foreach (var key in sorted.Keys)
                     {
                        if (sorted[key].Count > 0)
                        {
									// HACK? this was using i for the index, but was wrong

									eiRow.InsertAt(CreateCell(sorted[key][0]), key);
                        }
                     }

                     sheetData.AppendChild(eiRow);
                  }
               }
            }
         }

         return sheet;
      }

      private Sheet createCollectionTeamsSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = $"Collection Teams" };
         // sheets.Append(sheet);

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);


         Row headerRow = new Row();
         headerRow.Append(CreateCell("Index"));
         headerRow.Append(CreateCell("Name"));
         headerRow.Append(CreateCell("Team UUID"));
         headerRow.Append(CreateCell("Enumeration Area"));
         headerRow.Append(CreateCell("Enumeration Area UUID"));
         headerRow.Append(CreateCell("Polygon Latitude"));
         headerRow.Append(CreateCell("Polygon Longitude"));

         sheetData.AppendChild(headerRow);

         int i = 1;
         foreach (var enumArea in data.enumAreas)
         {
            foreach (var team in enumArea.collectionTeams)
            {
                    foreach (var pos in team.polygon)
                    {
                        Row teamRow = new Row();
                        teamRow.Append(CreateCell($"{i}"));
                        teamRow.Append(CreateCell(team.name));
                        teamRow.Append(CreateCell(team.uuid));
                        teamRow.Append(CreateCell(enumArea.name));
                        teamRow.Append(CreateCell(enumArea.uuid));
                        teamRow.Append(CreateCell($"{pos.latitude}"));
                        teamRow.Append(CreateCell($"{pos.longitude}"));
                        sheetData.AppendChild(teamRow);
                        i++;
                    }
                }
            }

         return sheet;
      }
      private Sheet createConfigurationSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = $"Configuration" };
         // sheets.Append(sheet);

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);

         Row headerRow = new Row();
         headerRow.Append(CreateCell("Name"));
			headerRow.Append(CreateCell("Creation Date"));
			headerRow.Append(CreateCell("Creation Time"));
			headerRow.Append(CreateCell("Distance Format"));
         headerRow.Append(CreateCell("Time Format"));
         headerRow.Append(CreateCell("Min GPS"));
         sheetData.AppendChild(headerRow);

			double unixTime = data.creationDate + data.timeZone * 60 * 60 * 1000;
			string date = ConvertTimeBasedOnTimezone(unixTime, TimeFormat.DATE);
			string time = ConvertTimeBasedOnTimezone(unixTime, TimeFormat.TIME);

			Row row = new Row();
         row.Append(CreateCell(data.name));
			row.Append(CreateCell(date));
			row.Append(CreateCell(time));
			row.Append(CreateCell(data.distanceFormat));
         row.Append(CreateCell(data.timeFormat));
         row.Append(CreateCell(data.minGpsPrecision.ToString()));
         sheetData.AppendChild(row);

         return sheet;
      }

      private Sheet createEnumerationAreaSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = "Enumeration Areas" };
         // sheets.Append(sheet);

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);
         Row headerRow = new Row();
         headerRow.Append(CreateCell("Index"));
         headerRow.Append(CreateCell("Name"));
         headerRow.Append(CreateCell("latitude"));
         headerRow.Append(CreateCell("longitude"));
         sheetData.AppendChild(headerRow);

         int i = 1;

         foreach (var ea in data.enumAreas)
         {
            foreach (var point in ea.vertices)
            {
               Row dataRow = new Row();
               dataRow.Append(CreateCell($"{i}"));
               dataRow.Append(CreateCell(ea.name));
               dataRow.Append(CreateCell($"{point.latitude}"));
               dataRow.Append(CreateCell($"{point.longitude}"));
               sheetData.AppendChild(dataRow);
               i++;
            }
         }
         return sheet;
      }

      private Sheet creatStudiesSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = "Studies" };
         // sheets.Append(sheet);

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);
         Row headerRow = new Row();
         headerRow.Append(CreateCell("Name"));
         headerRow.Append(CreateCell("Sampling Method"));
         headerRow.Append(CreateCell("Sample Type"));
         headerRow.Append(CreateCell("Sample Size"));
			headerRow.Append(CreateCell("field UUID"));
			headerRow.Append(CreateCell("Field Name"));
         headerRow.Append(CreateCell("Field Type"));
         headerRow.Append(CreateCell("field Parent UUID"));
         headerRow.Append(CreateCell("Field PII"));
         headerRow.Append(CreateCell("Field Required"));
         headerRow.Append(CreateCell("Field Integer Only"));
         headerRow.Append(CreateCell("Field Date"));
         headerRow.Append(CreateCell("Field Time"));
         headerRow.Append(CreateCell("Field Option"));
         // filters
         // find number of field operators


         sheetData.AppendChild(headerRow);

         foreach (var study in data.studies)
         {
            Row studyRow = new Row();
            studyRow.Append(CreateCell(study.name));
            studyRow.Append(CreateCell(study.samplingMethod));
            studyRow.Append(CreateCell(study.sampleType));
            studyRow.Append(CreateCell($"{study.sampleSize}"));

            //sheetData.Append(studyRow);

            foreach (var field in study.fields)
            {
					(studyRow, sheetData) = addField(field, studyRow, sheetData, 0);

					if (field.fields != null)
					{
						foreach (var childField in field.fields)
						{
							(studyRow, sheetData) = addField(childField, studyRow, sheetData, field.index);
						}
					}
				}
			}

         return sheet;
      }

      private (Row, SheetData) addField( GPSSampleDecoder.DataObjects.Field field, Row studyRow, SheetData sheetData, int parentFieldIndex )
      {
			if (field.fieldOptions.Count() > 0)
			{
				foreach (var fieldOption in field.fieldOptions)
				{
					studyRow.Append(CreateCell(field.uuid));
					studyRow.Append(CreateCell(field.index.ToString() + ". " + field.name));
					studyRow.Append(CreateCell(field.type));
					studyRow.Append(CreateCell($"{field.parentUUID}"));
					studyRow.Append(CreateCell($"{field.pii}"));
					studyRow.Append(CreateCell($"{field.required}"));
					studyRow.Append(CreateCell($"{field.integerOnly}"));
					studyRow.Append(CreateCell($"{field.date}"));
					studyRow.Append(CreateCell($"{field.time}"));
					studyRow.Append(CreateCell($"{fieldOption.name}"));

					sheetData.Append(studyRow);

					studyRow = new Row();
					studyRow.Append(CreateCell(""));
					studyRow.Append(CreateCell(""));
					studyRow.Append(CreateCell(""));
					studyRow.Append(CreateCell(""));
				}
			}
			else
			{
            var fieldName = field.index.ToString() + ". " + field.name;

            if (parentFieldIndex > 0)
               fieldName = parentFieldIndex.ToString() + "." + fieldName;

				studyRow.Append(CreateCell(field.uuid));
				studyRow.Append(CreateCell(fieldName));
				studyRow.Append(CreateCell(field.type));
				studyRow.Append(CreateCell($"{field.parentUUID}"));
				studyRow.Append(CreateCell($"{field.pii}"));
				studyRow.Append(CreateCell($"{field.required}"));
				studyRow.Append(CreateCell($"{field.integerOnly}"));
				studyRow.Append(CreateCell($"{field.date}"));
				studyRow.Append(CreateCell($"{field.time}"));

				sheetData.Append(studyRow);

				studyRow = new Row();
				studyRow.Append(CreateCell(""));
				studyRow.Append(CreateCell(""));
				studyRow.Append(CreateCell(""));
				studyRow.Append(CreateCell(""));
			}

         return (studyRow, sheetData);
		}

		private Sheet creatRulesSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = "Rules" };

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);
         Row headerRow = new Row();
         headerRow.Append(CreateCell("Study Name"));
         headerRow.Append(CreateCell("Rule Name"));
         headerRow.Append(CreateCell("Field Name"));
         headerRow.Append(CreateCell("Operator"));
         headerRow.Append(CreateCell("Value"));
         //headerRow.Append(CreateCell("Field choice used for rule"));
         sheetData.AppendChild(headerRow);
         // filters
         // find number of field operators
         foreach (var study in data.studies)
         {
            foreach(var rule in study.rules)
            {
					DataObjects.Field field = getField(rule.fieldUuid, data.studies);

					Row ruleRow = new Row();
               ruleRow.Append(CreateCell(study.name));
               ruleRow.Append(CreateCell(rule.name));
               ruleRow.Append(CreateCell(field.name));
               ruleRow.Append(CreateCell(rule.@operator));
               ruleRow.Append(CreateCell(rule.value));

               sheetData.Append(ruleRow);
            }

         }
         
         return sheet;
      }

      private Sheet creatFiltersSheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
      {
         WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
         Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = "Filters" };

         SheetData sheetData = new SheetData();
         wSheetPart.Worksheet = new Worksheet(sheetData);
         Row headerRow = new Row();
         headerRow.Append(CreateCell("Study Name"));
         headerRow.Append(CreateCell("Filter Name"));
         headerRow.Append(CreateCell("Rule"));
         headerRow.Append(CreateCell("Operator"));
         headerRow.Append(CreateCell("Rule"));
         // filters
         // find number of field operators

         sheetData.AppendChild(headerRow);

         foreach (var study in data.studies)
         {
            foreach (var filter in study.filters)
            {
               // build the rule chain
               List<DataObjects.Rule> rules = new List<DataObjects.Rule>();
               DataObjects.Rule rule = filter.rule;
               while(rule != null)
               {
                  rules.Add(rule);
                  if(rule.filterOperator != null)
                  {
                     rule = rule.filterOperator.rule;
                  }
                  else
                  {
                     rule = null;
                  }
               }
               int numRows = rules.Count();
               for(int i = 0; i < numRows; i++)
               {
                  Row filterRow = new Row();
                  DataObjects.Rule theRule = rules[i];
                  if(theRule != null)
                  {
                     filterRow.Append(CreateCell(study.name));
                     filterRow.Append(CreateCell(filter.name));
                     filterRow.Append(CreateCell(theRule.name));
                     if(theRule.filterOperator != null && theRule.filterOperator.rule != null)
                     {
                        filterRow.Append(CreateCell( theRule.filterOperator.connector));
                        filterRow.Append(CreateCell(theRule.filterOperator.rule.name));

                     }
                  }
                  sheetData.Append(filterRow);
               }
            }

         }

         return sheet;
      }

		private Sheet createSummarySheet(WorkbookPart wBookPart, SpreadsheetDocument spreadsheetDoc, uint sheetId, Configuration data)
		{
			WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
			Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = "EA Summary" };

			SheetData sheetData = new SheetData();
			wSheetPart.Worksheet = new Worksheet(sheetData);
			Row headerRow = new Row();
			headerRow.Append(CreateCell("Enumeration Area Name"));
			headerRow.Append(CreateCell("# of enumeration items"));
			headerRow.Append(CreateCell("# of multi-households"));
			headerRow.Append(CreateCell("# Enumerated"));
			headerRow.Append(CreateCell("# Incomplete - enumeration"));
			headerRow.Append(CreateCell("# Eligible for sampling"));
			headerRow.Append(CreateCell("# Sampled"));
			headerRow.Append(CreateCell("# Surveyed"));
			headerRow.Append(CreateCell("# Incomplete - survey"));
			headerRow.Append(CreateCell("# Remaining to survey"));
			headerRow.Append(CreateCell("# Points of Interest"));

			// filters
			// find number of field operators

			sheetData.AppendChild(headerRow);

			foreach (var enumArea in data.enumAreas)
         {
            int numEnumerationItems = 0;
            int numEnumerated = 0;
            int numEnumerationsIncomplete = 0;
            int numEligible = 0;
            int numSampled = 0;
            int numSurveyed = 0;
            int numSurveysIncomplete = 0;
            int numPointsOfInterest = 0;
            int numMultiHouseholds = 0;

            foreach (var location in enumArea.locations)
            {
               if (location.isLandmark)
               {
                  numPointsOfInterest += 1;
               }
               else
               {
						numEnumerationItems += location.enumerationItems.Count;

                  if (location.enumerationItems.Count > 1)
                  {
                     numMultiHouseholds += 1;
                  }

						foreach (var enumItem in location.enumerationItems)
						{
							if (enumItem.enumerationState == "Enumerated")
							{
								numEnumerated += 1;
							}
							if (enumItem.enumerationState == "Incomplete")
							{
								numEnumerationsIncomplete += 1;
							}
							if (enumItem.enumerationEligibleForSampling)
							{
								numEligible += 1;
							}
							if (enumItem.samplingState == "Sampled")
							{
								numSampled += 1;
							}
							if (enumItem.collectionState == "Complete")
							{
								numSurveyed += 1;
							}
							if (enumItem.collectionState == "Incomplete")
							{
								numSurveysIncomplete += 1;
							}
						}
					}
				}

            int numRemaining = numSampled - numSurveyed;

				Row row = new Row();

				row.Append(CreateCell(enumArea.name));
				row.Append(CreateCell(numEnumerationItems.ToString()));
				row.Append(CreateCell(numMultiHouseholds.ToString()));
				row.Append(CreateCell(numEnumerated.ToString()));
				row.Append(CreateCell(numEnumerationsIncomplete.ToString()));
				row.Append(CreateCell(numEligible.ToString()));
				row.Append(CreateCell(numSampled.ToString()));
				row.Append(CreateCell(numSurveyed.ToString()));
				row.Append(CreateCell(numSurveysIncomplete.ToString()));
				row.Append(CreateCell(numRemaining.ToString()));
				row.Append(CreateCell(numPointsOfInterest.ToString()));

            sheetData.Append(row);
			}

			return sheet;
		}

		public void CreateExcelFile(Configuration data, string OutPutFileDirectory)
      {
         var datetime = DateTime.Now.ToString().Replace(" /", "_").Replace(":", "_");

         string fileFullname = Path.Combine(OutPutFileDirectory, "Output.xlsx");

         if (File.Exists(fileFullname))
         {
            fileFullname = Path.Combine(OutPutFileDirectory, "Output_" + datetime + ".xlsx");
         }

         //using (SpreadsheetDocument package = SpreadsheetDocument.Create(fileFullname, SpreadsheetDocumentType.Workbook))
         //{
         //   CreateExcel(package, data);
         //}
      }

      private Cell CreateCell(string text)
      {
         Cell cell = new Cell();
         cell.DataType = CellValues.String;
         cell.CellValue = new CellValue(text);
         return cell;
      }
      private Cell CreateCell(string text, uint styleIndex)
      {
         Cell cell = new Cell();
         cell.StyleIndex = styleIndex;
         cell.DataType = CellValues.String;
         cell.CellValue = new CellValue(text);
         return cell;
      }

      private GPSSampleDecoder.DataObjects.Field getField( string uuid, List<Study> studies )
      {
			foreach (Study study in studies)
			{
				foreach (var field in study.fields)
				{
               if (field.uuid == uuid)
               {
                  return field;
               }
					else if (field.fields != null)
					{
						foreach (var childField in field.fields)
						{
							if (childField.uuid == uuid)
							{
								return field;
							}
						}
					}
				}
			}

			return null;
      }

      private string ConvertTimeBasedOnTimezone(double timeSinceEpoch, TimeFormat timeFormat)
      {
         double timeVal = timeSinceEpoch;
         TimeSpan time = TimeSpan.FromMilliseconds(timeVal);
         DateTime startdate = new DateTime(1970, 1, 1) + time;
         
         TimeZoneInfo thisTimeZone = TimeZoneInfo.Local;

         //string dateFormatted = startdate.ToString("MM/dd/yyyy HH:mm:ss");
         ////DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(startdate, thisTimeZone);
         //DateTime currentTime = TimeZoneInfo.ConvertTime(startdate, thisTimeZone);
         //dateFormatted = currentTime.ToString("MM/dd/yyyy HH:mm:ss");
         
         // TimeZoneInfo.ConvertTimeToUtc(startdate, timeZone);
         string dateFormatted = "";

         switch(timeFormat)
         {
            case TimeFormat.DATETIME:
               dateFormatted = startdate.ToString("MM/dd/yyyy HH:mm:ss");
               break;
            case TimeFormat.DATE:
               dateFormatted = startdate.ToString("MM/dd/yyyy");
               break;
            case TimeFormat.TIME:
               dateFormatted = startdate.ToString("H:mm:ss");
               break;
            default:
               dateFormatted = startdate.ToString("MM/dd/yyyy HH:mm:ss");
               break;
         }
         
         return dateFormatted;
      }
   }
}


