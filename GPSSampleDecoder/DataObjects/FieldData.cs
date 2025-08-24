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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class FieldData
   {
      public string uuid { get; set; }
		public long creationDate { get; set; }
		public String fieldUuid { get; set; }
      public string name { get; set; }
		public string type { get; set; }
		public string textValue { get; set; }
      public double? numberValue { get; set; }
      public long? dateValue { get; set; }
		public int? dropdownIndex { get; set; }
		public int blockNumber { get; set; }
		public List<FieldDataOption> fieldDataOptions { get; set; }

		public FieldData() { }

		public void merge(FieldData newFieldData)
      {
			// merge FieldDataOptions list

			List<FieldDataOption> newFieldDataOptions = new List<FieldDataOption>();

			foreach (FieldDataOption newFieldDataOption in newFieldData.fieldDataOptions)
			{
				bool found = false;

				foreach (FieldDataOption oldFieldDataOption in this.fieldDataOptions)
				{
					if (newFieldDataOption.uuid == oldFieldDataOption.uuid)
					{
						found = true;
						oldFieldDataOption.merge(newFieldDataOption);
					}
				}

				if (!found)
				{
					newFieldDataOptions.Add(newFieldDataOption);
				}
			}

			foreach (FieldDataOption fieldDataOption in newFieldDataOptions)
			{
				this.fieldDataOptions.Add(fieldDataOption);
			}

			this.blockNumber = newFieldData.blockNumber;
			this.numberValue = newFieldData.numberValue;
			this.textValue = newFieldData.textValue;
			this.dateValue = newFieldData.dateValue;
			this.dropdownIndex = newFieldData.dropdownIndex;
			this.name = newFieldData.name;
			this.type = newFieldData.type;		
		}
	}
}
