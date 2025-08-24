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

namespace GPSSampleDecoder.DataObjects
{
   public class Field
   {
      public string uuid { get; set; }
		public long creationDate { get; set; }
		public string parentUUID { get; set; }
		public int index { get; set; }
		public string name { get; set; }
      public string type { get; set; }
      public bool pii { get; set; }
      public bool required { get; set; }
		public bool integerOnly { get; set; }
		public bool numberOfResidents { get; set; }
		public bool date { get; set; }
      public bool time { get; set; }
      public double? minimum { get; set; }
      public double? maximum { get; set; }
      public List<FieldOption> fieldOptions{set; get; }
		public List<Field> fields { set; get; }

		public Field() { }
   }
}
