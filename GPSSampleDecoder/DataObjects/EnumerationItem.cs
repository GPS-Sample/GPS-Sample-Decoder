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
	public class EnumerationItem
	{
		public string uuid { get; set; }
		public long creationDate { get; set; }
		public int syncCode { get; set; }
		public double distance { get; set; }
		public string distanceUnits { get; set; }
		public string subAddress { get; set; }
		public string enumeratorName { get; set; }
		public string enumerationState { get; set; }
		public long enumerationDate { get; set; }
		public string enumerationIncompleteReason { get; set; }
		public string enumerationNotes { get; set; }
		public bool enumerationEligibleForSampling { get; set; }
		public string samplingState { get; set; }
		public string collectorName { get; set; }
		public string collectionState { get; set; }
		public long collectionDate { get; set; }
		public string collectionIncompleteReason { get; set; }
		public string collectionNotes { get; set; }
		public string locationUuid { get; set; }
		public string odkRecordUri { get; set; }
		public List<FieldData> fieldDataList { get; set; }

      public EnumerationItem() { }

		public void merge(EnumerationItem newEnumerationItem)
      {
			// merge FieldData list

			List<FieldData> newFieldDatas = new List<FieldData>();

			foreach (FieldData newFieldData in newEnumerationItem.fieldDataList)
			{
				bool found = false;

				foreach (FieldData oldFieldData in this.fieldDataList)
				{
					if (newFieldData.uuid == oldFieldData.uuid)
					{
						found = true;
						oldFieldData.merge(newFieldData);
					}
				}

				if (!found)
				{
					newFieldDatas.Add(newFieldData);
				}
			}

			foreach (FieldData fieldData in newFieldDatas)
			{
				this.fieldDataList.Add(fieldData);
			}

			this.creationDate = newEnumerationItem.creationDate;
			this.syncCode = newEnumerationItem.syncCode;
			this.subAddress = newEnumerationItem.subAddress;

			this.enumeratorName = newEnumerationItem.enumeratorName;
			this.enumerationState = newEnumerationItem.enumerationState;
			this.enumerationDate = newEnumerationItem.enumerationDate;
			this.enumerationIncompleteReason = newEnumerationItem.enumerationIncompleteReason;
			this.enumerationNotes = newEnumerationItem.enumerationNotes;
			this.enumerationEligibleForSampling = newEnumerationItem.enumerationEligibleForSampling;

			this.samplingState = newEnumerationItem.samplingState;
			this.collectorName = newEnumerationItem.collectorName;
			this.collectionState = newEnumerationItem.collectionState;
			this.collectionDate = newEnumerationItem.collectionDate;
			this.collectionIncompleteReason = newEnumerationItem.collectionIncompleteReason;
			this.collectionNotes = newEnumerationItem.collectionNotes;

			this.locationUuid = newEnumerationItem.locationUuid;
		}
	}
}
