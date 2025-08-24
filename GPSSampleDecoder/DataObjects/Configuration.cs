/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using GPSSampleDecoder.Static;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class Configuration
   {
		public string uuid { get; set; }
      public long creationDate { get; set; }
		public int timeZone { get; set; }
		public string name { get; set; }
		public int dbVersion { get; set; }
	   public string dateFormat { get; set; }
		public string timeFormat { get; set; }
		public string distanceFormat { get; set; }
		public int minGpsPrecision { get; set; }
		public string encryptionPassword { get; set; }
		public bool allowManualLocationEntry { get; set; }
		public bool subaddressIsrequired { get; set; }
		public bool autoIncrementSubaddress { get; set; }
		public bool proximityWarningIsEnabled { get; set; }
		public int proximityWarningValue { get; set; }
		public string selectedStudyUuid { get; set; }
		public string selectedEnumAreaUuid { get; set; }
		public List<Study> studies { get; set; }
		public List<EnumArea> enumAreas { get; set; }

		public Configuration() { }

		public void merge( Configuration newConfig )
      {
			// merge EnumAreas

			List<EnumArea> newEnumAreas = new List<EnumArea>();

			foreach (EnumArea newEnumArea in newConfig.enumAreas)
			{
				bool found = false;

				foreach (EnumArea oldEnumArea in this.enumAreas)
				{
					if (newEnumArea.uuid == oldEnumArea.uuid)
					{
						found = true;
						oldEnumArea.merge( newEnumArea );
					}
				}

				if (!found)
				{
					newEnumAreas.Add( newEnumArea );
				}
			}

			foreach (EnumArea enumArea in newEnumAreas)
			{
				this.enumAreas.Add( enumArea );
			}
		}
	}
}
