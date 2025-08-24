/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GPSSampleDecoder.DataObjects
{
   public class Rule
   {
      public string uuid { get; set; }
      public string fieldUuid { get; set; }
      public string name { get; set; }
      public string value { get; set; }
      public string @operator { get; set; }
      public FilterOperator filterOperator { get; set; }
      public List<FieldDataOption> fieldDataOptions { get; set; }

      public Rule() { }

   }
}
