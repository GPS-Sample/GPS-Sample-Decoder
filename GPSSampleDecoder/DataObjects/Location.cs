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
   public class Location
   {
		public string uuid { get; set; }
		public long creationDate { get; set; }
		public double timeZone { get; set; }
		public double distance { get; set; }
		public string distanceUnits { get; set; }
		public string type { get; set; }
		public int gpsAccuracy { get; set; }
      public double latitude { get; set; }
      public double longitude { get; set; }
      public double altitude { get; set; }
      public bool isLandmark { get; set; }
      public string description { get; set; }
      public string imageUuid { get; set; }
      public bool? isMultiFamily { get; set; }
      public string properties { get; set; }      
      public List<EnumerationItem> enumerationItems { get; set; }

      public Location() { }

		public void merge(Location newLocation)
      {
			// merge EnumerationItems

			List<EnumerationItem> newEnumerationItems = new List<EnumerationItem>();

			foreach (EnumerationItem newEnumerationItem in newLocation.enumerationItems)
			{
				bool found = false;

				foreach (EnumerationItem oldEnumerationItem in this.enumerationItems)
				{
					if (newEnumerationItem.uuid == oldEnumerationItem.uuid)
					{
						found = true;
						if (newEnumerationItem.syncCode > oldEnumerationItem.syncCode)
                  {
							oldEnumerationItem.merge(newEnumerationItem);
						}
					}
				}

				if (!found)
				{
					newEnumerationItems.Add(newEnumerationItem);
				}
			}

			foreach (EnumerationItem enumerationItem in newEnumerationItems)
			{
				this.enumerationItems.Add(enumerationItem);
			}
		}
	}
}
