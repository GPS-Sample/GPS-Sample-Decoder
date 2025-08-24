/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.Static
{

   public enum DateFormat
   {
      None,
      DayMonthYear,
      MonthDayYear,
      YearMonthDay,
   }

   public enum PasscodeAction
   {
      Ok,
      Cancel
   }
   public class PasscodeReturn
   {
      public PasscodeAction action { get; set; }
      public string passcode { get; set; }
      public PasscodeReturn() 
      {
         passcode = "";
         action = PasscodeAction.Cancel;
      }
   }

   public static class StaticStrings
   {
      public const string kAppTitle = "GPSSample Decoder";
      public const string kExitString = "Leave " + kAppTitle + "?";
      public const string kNoFileSelected = "No encrypted Configuration selected.\nPlease select a file.";
      public const string kNoDirectorySelected = "No save directory selected.\nPlease select a direcrtory.";

      public const string kNotValidConfig = "This file does not appear to be a correctly formatted Configuration file";
      public const string kInstructions = "Choose one or more configuration files.";
      public const string kOutputInstructions = "Choose the output directory.  The selected output " +
                                                  "will be saved to that directory with the same name as the Configuration.";
      public const string kDecodeComplete = "Decode Complete";
      public const string kSaveComplete = "Save Complete";
   }

   public static class DateFormatConverter
   {
      public static DateFormat FromString(string format)
      {
         DateFormat dateFormat;
         switch (format)
         {
            case "DayMonthYear":
               dateFormat = DateFormat.DayMonthYear;
               break;
            case "MonthDayYear":
               dateFormat = DateFormat.MonthDayYear;
               break;

            case "YearMonthDay":
               dateFormat = DateFormat.YearMonthDay;
               break;
            default:
               dateFormat = DateFormat.None;
               break;

         }
         return dateFormat;
      }
      public static string ToString(DateFormat dateFormat)
      {
         string dateFormatStr = "None";
         switch (dateFormat)
         {
            case DateFormat.DayMonthYear:
               dateFormatStr = "DayMonthYear";
               break;
            case DateFormat.MonthDayYear:
               dateFormatStr = "MonthDayYear";
               break;
            case DateFormat.YearMonthDay:
               dateFormatStr = "YearMonthDay";
               break;
            case DateFormat.None:
               dateFormatStr = "None";
               break;

         }
         return dateFormatStr;
      }
   }

   public enum SaveState
   {
      Invalid,
      Csv,
      Xls,
      CsvAndXls
   }

   public enum SaveStateError
   {
      Invalid,
      Success,
      Failed
   }


//   enum class Connector(val format : String) {
//      NONE("NONE"),
//    AND("AND"),
//    OR("OR"),
//    NOT("NOT"),
//}

   public enum Connector
   {
      NONE,
      AND,
      OR,
      NOT
   }

   public static class ConnectorConverter
   {
      public static Connector FromString(string connector)
      {
         Connector _connector = Connector.NONE;
         switch (connector)
         {
            case "NONE":
               _connector = Connector.NONE;
               break;
            case "AND":
               _connector = Connector.AND;
               break;
            case "OR":
               _connector = Connector.OR;
               break;
            case "NOT":
               _connector = Connector.NOT;
               break;
            default:
               _connector = Connector.NONE; 
               break;
         }
         return _connector;
      }
      public static string ToString(Connector connector)
      {
         string _connectorString = "NONE";
         switch(connector)
         {
            case Connector.NONE:
               _connectorString = "NONE";
               break;
            case Connector.AND:
               _connectorString = "AND";
               break;
            case Connector.OR:
               _connectorString = "OR";
               break;
            case Connector.NOT:
               _connectorString = "NOT";
               break;
            default:
               _connectorString = "NONE";
               break;
         }
         return _connectorString;
      }
   }

   public enum Operator
   {
      None,
      Equal,
      NotEqual,
      LessThan,
      GreaterThan,
      LessThanOrEqual,
      GreaterThanOrEqual,
   }

   public static class OperatorConverter
   {
      public static Operator FromString(string theOperator)
      {
         Operator _operator = Operator.None;
         switch (theOperator)
         {
            case "None":
               _operator = Operator.None;
               break;
            case "Equal":
               _operator = Operator.Equal;
               break;
            case "NotEqual":
               _operator = Operator.NotEqual;
               break;
            case "LessThan":
               _operator = Operator.LessThan;
               break;
            case "GreaterThan":
               _operator = Operator.GreaterThan;
               break;
            case "LessThanOrEqual":
               _operator = Operator.LessThanOrEqual;
               break;
            case "GreaterThanOrEqual":
               _operator = Operator.GreaterThanOrEqual;
               break;
            default:
               _operator = Operator.None;
               break;


         }
         return _operator;
      }

      public static string ToString(Operator theOperator)
      {
         switch(theOperator)
         {
            case Operator.None:
               return "None";
               
            case Operator.Equal:
               return "Equal";
            case Operator.NotEqual:
               return "NotEqual";
            case Operator.LessThan:
               return "LessThan";
            case Operator.GreaterThan:
               return "GreaterThan";
            case Operator.LessThanOrEqual:
               return "LessThanOrEqual";
            case Operator.GreaterThanOrEqual:
               return "GreaterThanOrEqual";
            default: return "None";

         }
      }
   }

   public enum Role
   {
      Undefined,
      Admin,
      Supervisor,
      Enumerator,
      DataCollector
   }

   public static class RoleConverter
   {
      public static Role FromString(string role)
      {
         Role _role = Role.Undefined;
         switch (role)
         {
            case "Undefined":
               _role = Role.Undefined;
               break;
            case "Admin":
               _role = Role.Admin;
               break;
            case "Supervisor":
               _role = Role.Supervisor;
               break;
            case "Enumerator":
               _role = Role.Enumerator;
               break;
            case "DataCollector":
               _role = Role.DataCollector;
               break;
            default:
               _role = Role.Undefined;
               break;
         }
         return _role;
      }

      public static string ToString(Role role)
      {
         string rolestr = "None";
         switch (role)
         {
            case Role.Undefined:
               rolestr = "Undefined";
               break;
            case Role.Admin:
               rolestr = "Admin";
               break;
            case Role.Supervisor:
               rolestr = "Supervisor";
               break;
            case Role.Enumerator:
               rolestr = "Enumerator";
               break;
            default:
               rolestr = "Undefined";
               break;

         }
         return rolestr;
      }
   }

   public enum SampleType
   {
      None,
      NumberHouseholds,
      PercentHouseholds,
      PercentTotal,

   }

   public static class SampleTypeConverter
   {
      public static SampleType FromString(string type)
      {
         SampleType sampletype = SampleType.None;
         switch (type)
         {
            case "None":
               sampletype = SampleType.None;
               break;
            case "NumberHouseholds":
               sampletype = SampleType.NumberHouseholds;
               break;
            case "PercentHouseholds":
               Console.WriteLine("should be Percenthous");
               sampletype = SampleType.PercentHouseholds;
               break;
            case "PercentTotal":
               sampletype = SampleType.PercentTotal;
               break;
            default:
               sampletype = SampleType.None;
               break;
         }
         return sampletype;
      }

      public static string ToString(SampleType sampletype)
      {
         string sampletypestr = "";
         switch (sampletype)
         {
            case SampleType.None:
               sampletypestr = "None";
               break;
            case SampleType.NumberHouseholds:
               sampletypestr = "# of Households";
               break;
            case SampleType.PercentHouseholds:
               sampletypestr = "% of all Households";
               break;
            case SampleType.PercentTotal:
               sampletypestr = "% of total population";
               break;
            default:
               sampletypestr = "None";
               break;
         }

         return sampletypestr;
      }
   }

   public enum SamplingMethod
   {
      None,
      SimpleRandom,
      Cluster,
      Subsets,
      Strata,
   }

   public static class SamplingMethodConverter
   {
      public static SamplingMethod FromString(string method)
      {
         SamplingMethod samplingMethod = SamplingMethod.None;
         switch (method)
         {
            case "None":
               samplingMethod = SamplingMethod.None;
               break;
            case "SimpleRandom":
               samplingMethod = SamplingMethod.SimpleRandom;
               break;
            case "Cluster":
               samplingMethod = SamplingMethod.Cluster;
               break;
            case "Subsets":
               samplingMethod = SamplingMethod.Subsets;
               break;
            case "Strata":
               samplingMethod = SamplingMethod.Strata;
               break;
            default:
               samplingMethod = SamplingMethod.None;
               break;
         }
         return samplingMethod;
      }
      public static string ToString(SamplingMethod samplingMethod)
      {
         string samplingMethodStr = "";
         switch (samplingMethod)
         {
            case SamplingMethod.None:
               samplingMethodStr = "None";
               break;
            case SamplingMethod.SimpleRandom:
               samplingMethodStr = "SimpleRandom";
               break;
            case SamplingMethod.Cluster:
               samplingMethodStr = "Cluster";
               break;
            case SamplingMethod.Subsets:
               samplingMethodStr = "Subsets";
               break;
            case SamplingMethod.Strata:
               samplingMethodStr = "Strata";
               break;
            default:
               samplingMethodStr = "None";
               break;
         }
         return samplingMethodStr;
      }
   }

   public enum FieldType
   {
      None,
      Text,
      Number,
      Date,
      Checkbox,
      Dropdown,
   }

   public static class FieldTypeConverter
   {
      public static FieldType FromString(string type)
      {
         FieldType fieldType = FieldType.None;
         switch (type)
         {
            case "None":
               fieldType = FieldType.None;
               break;
            case "Text":
               fieldType = FieldType.Text;
               break;
            case "Number":
               fieldType = FieldType.Number;
               break;
            case "Date":
               fieldType = FieldType.Date;
               break;
            case "Checkbox":
               fieldType = FieldType.Checkbox;
               break;
            case "Dropdown":
               fieldType = FieldType.Dropdown;
               break;
            default:
               fieldType = FieldType.None;
               break;

         }
         return fieldType;
      }

		public static string ToString(LocationType locationType)
      {
         string locationFormatStr = "None";
         switch (locationType)
         {
            case LocationType.Sample:
               locationFormatStr = "Sample";
               break;
            case LocationType.Enumeration:
               locationFormatStr = "Enumeration";
               break;
            case LocationType.None:
               locationFormatStr = "None";
               break;
         }
         return locationFormatStr;
      }
   }






   public enum LocationType
   {
      None,
      Sample,
      Enumeration,
   }

   public static class LocationTypeConverter
   {
      public static LocationType FromString(string type)
      {
         LocationType locationType = LocationType.None;
         switch (type)
         {
            case "None":
               locationType = LocationType.None;
               break;
            case "Sample":
               locationType = LocationType.Sample;
               break;
            case "Enumeration":
               locationType = LocationType.Enumeration;
               break;
            default:
               locationType = LocationType.None;
               break;

         }
         return locationType;
      }

      public static string ToString(LocationType locationType)
      {
         string locationFormatStr = "None";
         switch (locationType)
         {
            case LocationType.Sample:
               locationFormatStr = "Sample";
               break;
            case LocationType.Enumeration:
               locationFormatStr = "Enumeration";
               break;
            case LocationType.None:
               locationFormatStr = "None";
               break;
         }
         return locationFormatStr;
      }
   }

   public enum TimeFormat
   {
      None,
      twelveHour,
      twentyFourHour,
   }
   public static class TimeFormatConverter
   {
      public static TimeFormat FromString(string format)
      {
         TimeFormat timeFormat;
         switch (format)
         {
            case "twelveHour":
               timeFormat = TimeFormat.twelveHour;
               break;
            case "twentyFourHour":
               timeFormat = TimeFormat.twentyFourHour;
               break;

            default:
               timeFormat = TimeFormat.None;
               break;

         }
         return timeFormat;
      }

      public static string ToString(TimeFormat timeFormat)
      {
         string timeFormatStr = "None";
         switch (timeFormat)
         {
            case TimeFormat.twelveHour:
               timeFormatStr = "twelveHour";
               break;
            case TimeFormat.twentyFourHour:
               timeFormatStr = "twentyFourHour";
               break;
            case TimeFormat.None:
               timeFormatStr = "None";
               break;
         }
         return timeFormatStr;
      }
   }


   public enum DistanceFormat
   {
      None,
      Meters,
      Feet,
      Kilometers,
      Miles,
   }
   public static class DistanceFormatConverter
   {
      public static DistanceFormat FromString(string format)
      {
         DistanceFormat distanceFormat;
         switch (format)
         {
            case "Meters":
               distanceFormat = DistanceFormat.Meters;
               break;
            case "Kilometers":
               distanceFormat = DistanceFormat.Kilometers;
               break;
            case "Meters / Kilometers":
               distanceFormat = DistanceFormat.Meters;
               break;
            case "Feet":
               distanceFormat = DistanceFormat.Feet;
               break;
            case "Miles":
               distanceFormat = DistanceFormat.Miles;
               break;
            case "Feet / Miles":
               distanceFormat = DistanceFormat.Feet;
               break;

            default:
               distanceFormat = DistanceFormat.None;
               break;

         }
         return distanceFormat;
      }
      public static string ToString(DistanceFormat timeFormat)
      {
         string timeFormatStr = "None";
         switch (timeFormat)
         {
            case DistanceFormat.Meters:
               timeFormatStr = "Meters / Kilometers";
               break;
            case DistanceFormat.Feet:
               timeFormatStr = "Feet / Miles";
               break;
            case DistanceFormat.None:
               timeFormatStr = "None";
               break;
         }
         return timeFormatStr;
      }
   }

   public static class Macros
   {

   }
}
