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

namespace GPSSampleDecoder.DataObjects
{
   public class Filter
   {
		public string uuid { get; set; }
      public string name { get; set; }
      public string samplingType { get; set; }
      public int sampleSize { get; set; }
      public Rule rule { get; set; }

      public Filter() { }
   }
}
