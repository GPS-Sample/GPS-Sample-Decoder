/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using DocumentFormat.OpenXml.Packaging;
using GPSSampleDecoder.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.Delegates
{
   public delegate void DecodePercentDone(int percent);
   public delegate void DecodeCompleted(RunWorkerCompletedEventArgs e, string rawJSON, List<Configuration>configurations, ImageList imageList);
   public delegate void DecodeError(string message);

   public delegate void SavePercentDone(int percent);
   public delegate void SaveCompleted(RunWorkerCompletedEventArgs e);
   public delegate void SaveError(string message);

}
