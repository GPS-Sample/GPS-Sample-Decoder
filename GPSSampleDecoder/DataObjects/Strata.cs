/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.Static;

namespace GPSSampleDecoder.DataObjects
{
    public class Strata
    {
        public string uuid { get; set; }
        public long? creationDate { get; set; }
        public string studyUuid { get; set; }
        public string name { get; set; }
        public int sampleSize { get; set; }
        public string sampleType { get; set; }

        public Strata() { }
    }
}
