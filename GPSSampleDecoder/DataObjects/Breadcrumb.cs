/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/

using System;

namespace GPSSampleDecoder.DataObjects
{
	public class Breadcrumb
	{
        public string uuid { get; set; }
        public long creationDate { get; set; }
        public string enumAreaUuid { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string groupId { get; set; }

        public Breadcrumb() {}
	}
}