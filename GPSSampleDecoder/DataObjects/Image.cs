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
	public class Image
	{
		public string uuid { get; set; }
		public long creationDate { get; set; }
		public string locationUuid { get; set; }
		public string data { get; set; }

		public Image() { }
	}
}
