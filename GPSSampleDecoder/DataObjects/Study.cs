/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.Static;
using System.Collections.Generic;

namespace GPSSampleDecoder.DataObjects
{
   public class Study
   {
		public string uuid { get; set; }
		public long? creationDate { get; set; }
		public string name { get; set; }
        public string subsetSampleName { get; set; }
        public string samplingMethod { get; set; }
		public int sampleSize { get; set; }
        public int subsetSampleSize { get; set; }
        public string sampleType { get; set; }
        public string subsetSampleType { get; set; }
        public List<Strata> stratas { get; set; }
        public List<Field> fields { get; set; }
        public List<Rule> rules { get; set; }
        public List<Filter> filters { get; set; }
        public List<Rule> primaryRules { get; set; }
		public List<Filter> primaryFilters { get; set; }
        public List<Rule> subsetRules { get; set; }
        public List<Filter> subsetFilters { get; set; }

        public Study() { }
   }
}
