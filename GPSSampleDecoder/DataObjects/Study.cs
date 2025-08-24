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
   public class Study
   {
      private SamplingMethod _samplingMethod = SamplingMethod.None;
      private SampleType _sampleType = SampleType.None;

		public string uuid { get; set; }
      public long? creationDate { get; set; }
      public string name { get; set; }
		public string samplingMethod { get; set; }
		public int sampleSize { get; set; }
      public string sampleType { get; set; }
		public List<Field> fields { get; set; }
		public List<Rule> rules { get; set; }
		public List<Filter> filters { get; set; }

		public Study() { }
   }
}
