/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

using GPSSampleDecoder.DataObjects;
using System.IO;
namespace GPSSampleDecoder.Utils
{
   public class ExcelWriter
   {
      private string _outputPath;
      ExcelUtils _excelUtils;
      public ExcelWriter() 
      {
         _excelUtils = new ExcelUtils();
      }

      public void SaveExcel(Configuration configuration, string outputPath)
      {
         _outputPath = outputPath;
         _excelUtils.ProcessExcel(configuration, _outputPath);

         //SLDocument sl = new SLDocument();
         //// set a boolean at "A1"
         //sl.SetCellValue("A1", true);

         //// set at row 2, columns 1 through 20, a value that's equal to the column index
         //for (int i = 1; i <= 20; ++i) sl.SetCellValue(2, i, i);

         //// set the value of PI
         //sl.SetCellValue("B3", 3.14159);

         //// set the value of PI at row 4, column 2 (or "B4") in string form.
         //// use this when you already have numeric data in string form and don't
         //// want to parse it to a double or float variable type
         //// and then set it as a value.
         //// Note that "3,14159" is invalid. Excel (or Open XML) stores numerals in
         //// invariant culture mode. Frankly, even "1,234,567.89" is invalid because
         //// of the comma. If you can assign it in code, then it's fine, like so:
         //// double fTemp = 1234567.89;
         //sl.SetCellValueNumeric(4, 2, "3.14159");

         //// normal string data
         //sl.SetCellValue("C6", "This is at C6!");

         //// typical XML-invalid characters are taken care of,
         //// in particular the & and < and >
         //sl.SetCellValue("I6", "Dinner & Dance costs < $10");

         //// this sets a cell formula
         //// Note that if you want to set a string that starts with the equal sign,
         //// but is not a formula, prepend a single quote.
         //// For example, "'==" will display 2 equal signs
         //sl.SetCellValue(7, 3, "=SUM(A2:T2)");

         //// if you need cell references and cell ranges *really* badly, consider the SLConvert class.
         //sl.SetCellValue(SLConvert.ToCellReference(7, 4), string.Format("=SUM({0})", SLConvert.ToCellRange(2, 1, 2, 20)));

         //// dates need the format code to be displayed as the typical date.
         //// Otherwise it just looks like a floating point number.
         //sl.SetCellValue("C8", new DateTime(3141, 5, 9));
         //SLStyle style = sl.CreateStyle();
         //style.FormatCode = "d-mmm-yyyy";
         //sl.SetCellStyle("C8", style);

         //sl.SetCellValue(8, 6, "I predict this to be a significant date. Why, I do not know...");

         //sl.SetCellValue(9, 4, 456.123789);
         //// we don't have to create a new SLStyle because
         //// we only used the FormatCode property
         //style.FormatCode = "0.000%";
         //sl.SetCellStyle(9, 4, style);

         //sl.SetCellValue(9, 6, "Perhaps a phenomenal growth in something?");

         //string outfile = Path.Combine(_outputPath, "HelloWorld.xlsx");
         //sl.SaveAs(outfile);

      }
   }
}
